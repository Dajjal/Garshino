import asyncio
import os

import aiohttp

from dotenv import load_dotenv

from database.database import Session
from first_install import AutoGraphUser


async def send_telegram_messages():
    print('sending telegram messages')
    token = os.getenv('BOT_TOKEN')
    url = f'https://api.telegram.org/bot{token}/sendMessage'

    with Session() as db_session:
        users = db_session.query(AutoGraphUser).all()

        async with aiohttp.ClientSession() as session:
            for user in users:
                data = {
                    'chat_id': user.telegram_chat_id,
                    'text': f"Заранее прошу прощения..\nСейчас будет спам..\nИ всё что было выслано за сегодня, "
                            f"повторно вышлется обратно.."
                }
                async with session.post(url, data=data) as response:
                    if response.status == 200:
                        print(user.fullname, ' message sent')


async def start():
    await send_telegram_messages()


if __name__ == '__main__':
    load_dotenv()
    asyncio.run(start())
