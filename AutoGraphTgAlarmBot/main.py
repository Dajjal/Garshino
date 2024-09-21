import asyncio
import json
import logging
import os
import sys
import uuid
from datetime import datetime, timedelta
from time import sleep

import aiohttp
from dotenv import load_dotenv
from sqlalchemy import and_

from database.database import Session
from first_install import AutoGraphStage, AutoGraphUser, AutoGraphDrivers, AutoGraphMachines


async def send_telegram_messages():
    print('sending telegram messages')
    token = os.getenv('BOT_TOKEN')
    fuel_tanker_ids = [uuid.UUID(tanker) for tanker in os.getenv('FUEL_TANKER_IDS', '*').split(';')]
    url = f'https://api.telegram.org/bot{token}/sendMessage'

    with Session() as db_session:
        messages = db_session.query(AutoGraphStage).filter(AutoGraphStage.sent == False).all()
        users = db_session.query(AutoGraphUser).all()

        async with aiohttp.ClientSession() as session:
            for message in messages:
                stage_icon = '🟢' if str(message.stage).lower() == 'заправка' else '🔴'
                dates = message.datetime.split(' - ')

                for user in users:
                    if message.machine_id in fuel_tanker_ids:
                        data = {
                            'chat_id': user.telegram_chat_id,
                            'text': f"🏢 - Гаршино\n{stage_icon} - {message.stage}\n"
                                    f"🚗 - {message.machine_name} - {message.machine_reg_number}\n"
                                    f"⌚ - {dates[0]}\n         {dates[1]}\n"
                                    f"⛽ - {message.benzo} л.\n👤 - {message.driver_fullname}\n"
                        }
                    else:
                        data = {
                            'chat_id': user.telegram_chat_id,
                            'text': f"🏢 - Гаршино\n{stage_icon} - {message.stage}\n"
                                    f"🚗 - {message.machine_name} - {message.machine_reg_number}\n"
                                    f"⌚ - {dates[0]}\n         {dates[1]}\n"
                                    f"⛽ - {message.benzo} л."
                        }

                    async with session.post(url, data=data) as response:
                        if response.status == 200:
                            message.sent = True
                            db_session.commit()


async def process_stage(session, token, schema, machines, stage_name):
    print('load stage')
    utc_timedelta = int(os.getenv('UTC_TIME_DELTA'))
    fuel_tanker_ids = os.getenv('FUEL_TANKER_IDS', '*').split(';')

    async with session.post(
            'https://auto.autograph.kz/ServiceJSON/GetStage',
            data={
                'session': token,
                'schemaID': schema,
                'IDs': ', '.join([m['id'] for m in machines]),
                'SD': datetime.now().strftime('%Y%m%d') + '-0000',
                'ED': datetime.now().strftime('%Y%m%d') + '-2359',
                'stageName': stage_name
            }
    ) as response:
        if response.status == 200:
            data = await response.json()
            is_refill = stage_name == os.getenv('STAGE_REFILL')
            machines = [m for m in machines if (m['id'] in fuel_tanker_ids) == is_refill]

            for machine in machines:
                machine_data = data.get(machine['id'])
                if not machine_data:
                    continue

                for item in machine_data.get('Items', []):
                    sd, ed = [datetime.strptime(item[t], "%Y-%m-%dT%H:%M:%S") + timedelta(hours=utc_timedelta)
                              for t in ('SD', 'ED')]
                    values = item.get('Values', [])
                    benzo = values[43] if is_refill and len(values) > 43 else values[28] if len(values) > 28 else 0
                    benzo = abs(round(benzo, 2))
                    driver_id = uuid.UUID(values[42]) if len(values) > 42 and values[42] != 0 else uuid.UUID(int=0)
                    driver_fullname = 'Водитель не найден'

                    if (ed + timedelta(minutes=int(os.getenv('ED_PLUS_IN_MINUTES')))) < datetime.now():
                        with Session() as db_session:
                            if not db_session.query(AutoGraphStage).filter(and_(
                                    AutoGraphStage.stage == item.get('Caption'),
                                    AutoGraphStage.datetime == f"{sd:%d.%m.%Y %H:%M:%S} - {ed:%d.%m.%Y %H:%M:%S}",
                                    AutoGraphStage.machine_id == uuid.UUID(machine['id']),
                                    AutoGraphStage.benzo == benzo,
                                    AutoGraphStage.driver_id == driver_id
                            )).first():
                                if driver_id != uuid.UUID(int=0):
                                    driver = db_session.query(AutoGraphDrivers).filter(
                                        AutoGraphDrivers.driver_id == driver_id).first()
                                    if driver is not None:
                                        driver_fullname = driver.driver_fullname
                                sent = False
                                if machine['id'] not in fuel_tanker_ids:
                                    if str(item.get('Caption')).lower() == 'заправка':
                                        sent = True
                                db_session.add(AutoGraphStage(
                                    stage=item.get('Caption'),
                                    datetime=f"{sd:%d.%m.%Y %H:%M:%S} - {ed:%d.%m.%Y %H:%M:%S}",
                                    machine_id=uuid.UUID(machine['id']),
                                    machine_name=machine['machine_name'],
                                    machine_reg_number=machine['reg_number'],
                                    benzo=benzo,
                                    driver_id=driver_id,
                                    driver_fullname=driver_fullname,
                                    sent=sent
                                ))
                                db_session.commit()


async def load_stages(token, schema, machines):
    async with aiohttp.ClientSession() as session:
        for stage in [os.getenv('STAGE_REFILL'), os.getenv('STAGE_DISCHARGE')]:
            sleep(5)
            await process_stage(session, token, schema, machines, stage)


async def load_machines(token):
    print('loading machines')
    schema = os.getenv('SCHEMA_ID')
    machine_ids = os.getenv('MACHINE_IDS', '*').split(';')

    async with aiohttp.ClientSession() as session:
        async with session.post(
                'https://auto.autograph.kz/ServiceJSON/EnumDevices',
                data={'session': token, 'schemaID': schema}
        ) as response:
            if response.status == 200:
                items = (await response.json()).get('Items', [])

                with open('machines.json', 'w', encoding='utf-8') as f:
                    json.dump((await response.json()), f, ensure_ascii=False, indent=4)

                with Session() as db_session:
                    for item in items:
                        if not db_session.query(AutoGraphMachines).filter(and_(
                                AutoGraphMachines.machine_id == uuid.UUID(item['ID']),
                                AutoGraphMachines.machine_name == item['Name'],
                                AutoGraphMachines.machine_reg_number == next((p['Value'] for p in item['Properties']
                                                                              if p['Name'] == 'VehicleRegNumber'), None)
                        )).first():
                            db_session.add(AutoGraphMachines(
                                machine_id=uuid.UUID(item['ID']),
                                machine_name=item['Name'],
                                machine_reg_number=next((p['Value'] for p in item['Properties']
                                                         if p['Name'] == 'VehicleRegNumber'), None)
                            ))
                            db_session.commit()

                machines = [
                    {'id': item['ID'], 'machine_name': item['Name'],
                     'reg_number': next((p['Value'] for p in item['Properties']
                                         if p['Name'] == 'VehicleRegNumber'), None)}
                    for item in items
                ]
                await load_stages(token, schema, [m for m in machines if '*' in machine_ids or m['id'] in machine_ids])


async def load_drivers(token):
    print('load drivers')
    schema = os.getenv('SCHEMA_ID')
    async with aiohttp.ClientSession() as session:
        async with session.post(
                'https://auto.autograph.kz/ServiceJSON/EnumDrivers',
                data={'session': token, 'schemaID': schema}
        ) as response:
            if response.status == 200:
                drivers = await response.json()
                with Session() as db_session:
                    for driver in drivers['Items']:
                        driver_id = uuid.UUID(driver.get('ID'))
                        if not db_session.query(AutoGraphDrivers).filter(and_(
                                AutoGraphDrivers.driver_string_id == driver.get('DriverID'),
                                AutoGraphDrivers.driver_id == driver_id,
                                AutoGraphDrivers.driver_fullname == driver.get('Name')
                        )).first():
                            db_session.add(AutoGraphDrivers(
                                driver_string_id=driver.get('DriverID'),
                                driver_id=driver_id,
                                driver_fullname=driver.get('Name')
                            ))
                            db_session.commit()


async def update_data():
    print('authorize')
    username, password = os.getenv('AUTOGRAPH_USERNAME'), os.getenv('AUTOGRAPH_PASSWORD')
    async with aiohttp.ClientSession() as session:
        async with session.post(
                'https://auto.autograph.kz/ServiceJSON/Login',
                data={'UserName': username, 'Password': password}
        ) as response:
            if response.status == 200:
                token = await response.text()
                sleep(5)
                await load_drivers(token)
                await load_machines(token)


async def periodic_task():
    while True:
        await update_data()
        await send_telegram_messages()
        await asyncio.sleep(int(os.getenv('UPDATE_INTERVAL_IN_SECONDS')))


async def start():
    await periodic_task()


if __name__ == '__main__':
    logging.basicConfig(level=logging.INFO, stream=sys.stdout)
    load_dotenv()

    try:
        asyncio.run(start())
    except Exception as e:
        logging.error(e)
