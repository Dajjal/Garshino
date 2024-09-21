import asyncio
import logging
import os
import sys
import uuid

from aiogram import Bot, Dispatcher, types
from dotenv import load_dotenv

from database.database import Session
from first_install import AutoGraphStage
from handlers.user import private_user_router


async def run_bot() -> None:
    print('bot run..')
    # disabling answering to previous messages
    await bot.delete_webhook(drop_pending_updates=True)

    # region Commands

    # private user commands
    await bot.delete_my_commands(scope=types.BotCommandScopeAllPrivateChats())

    # endregion

    # start listening
    await dispatcher.start_polling(bot)


if __name__ == '__main__':
    # logging
    logging.basicConfig(level=logging.INFO, stream=sys.stdout)

    # loading environments
    load_dotenv()

    # create tables in database if necessary
    session = Session()
    try:
        # check for database exists
        check = session.get(AutoGraphStage, uuid.UUID(int=0))
        session.close()

        # initializing bot and dispatcher
        bot: Bot = Bot(token=os.getenv('BOT_TOKEN'))
        dispatcher: Dispatcher = Dispatcher()

        # region Routers

        # private user routers
        dispatcher.include_routers(private_user_router)

        # endregion

        # run bot asynchronously
        asyncio.run(run_bot())
    except Exception as e:
        session.close()
        logging.error(e)
        logging.error(
            'У вас не создана таблица, для логирования данных..\n'
            'Чтобы её создать запустите сначала файл:\n'
            'python3 first_install.py - на linux & macos\n'
            'или для windows:\n'
            'python first_install.py')
