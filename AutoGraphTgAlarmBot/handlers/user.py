from aiogram import Router, filters, types, F
from sqlalchemy import and_

from database.database import Session
from first_install import AutoGraphUser

private_user_router = Router()


def check_user(chat_id, username, fullname):
    with Session() as db_session:
        existing_record = db_session.query(AutoGraphUser).filter(and_(
            AutoGraphUser.telegram_chat_id == chat_id
        )).first()

        if existing_record:
            return

        # Создание и добавление новой записи
        record = AutoGraphUser(
            telegram_chat_id=chat_id,
            username=username,
            fullname=fullname
        )
        db_session.add(record)
        db_session.commit()


@private_user_router.message(filters.CommandStart())
async def on_start_command(message: types.Message) -> None:
    await message.answer('👋')
    await message.answer('Добро пожаловать в бот компании AutoGraph Казахстан.')
    check_user(str(message.chat.id), message.chat.username, message.chat.full_name)


@private_user_router.message((F.text.lower() == 'id') | (F.text.lower() == 'ид'))
@private_user_router.message(filters.Command('my_id'))
async def on_my_id_command(message: types.Message) -> None:
    await message.answer('🆔 - ' + str(message.chat.id))
    check_user(str(message.chat.id), message.chat.username, message.chat.full_name)


@private_user_router.message(filters.Command('about'))
async def on_about_command(message: types.Message) -> None:
    await message.answer('Бот функционирует как средство уведомления о пополнениях и сливах типлива.')
    check_user(str(message.chat.id), message.chat.username, message.chat.full_name)


@private_user_router.message()
async def on_echo(message: types.Message) -> None:
    await message.answer('😭')
    await message.answer('''
Эта функция пока не доступна. Если она необходима, свяжитесь с разработчиком для обсуждения и поддержки проекта. 
Контакты:
`+7 (778) 780-99-43` — Керимбай
Telegram: @apikaster
Он также может подключить ИИ для взаимодействия с вами.
    ''', parse_mode='Markdown')
    check_user(str(message.chat.id), message.chat.username, message.chat.full_name)
