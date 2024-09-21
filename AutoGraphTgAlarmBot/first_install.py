import uuid

from sqlalchemy import Column, String, Boolean, Uuid

from database.database import Base, engine, Session


class BaseModel(Base):
    __abstract__ = True
    id = Column(Uuid, primary_key=True, default=uuid.uuid4)


class AutoGraphStage(BaseModel):
    __tablename__ = 'autograph_stages'

    stage = Column(String(50))  # заправка / слив
    datetime = Column(String(200))  # дата и время
    machine_id = Column(Uuid, default=uuid.UUID(int=0))  # id машины
    machine_name = Column(String(50))  # название машины
    machine_reg_number = Column(String(50))  # номер машины
    benzo = Column(String(10))  # литры бензина
    driver_id = Column(Uuid, default=uuid.UUID(int=0))  # id водителя
    driver_fullname = Column(String(100))  # фио водителя
    sent = Column(Boolean, default=False)  # отправлено в телеграм


class AutoGraphDrivers(BaseModel):
    __tablename__ = 'autograph_drivers'

    driver_string_id = Column(String(200))
    driver_id = Column(Uuid, default=uuid.UUID(int=0))
    driver_fullname = Column(String(200))


class AutoGraphMachines(BaseModel):
    __tablename__ = 'autograph_machines'

    machine_id = Column(Uuid, default=uuid.UUID(int=0))
    machine_name = Column(String(50))
    machine_reg_number = Column(String(50))



class AutoGraphUser(BaseModel):
    __tablename__ = 'autograph_users'

    telegram_chat_id = Column(String(50))
    username = Column(String(50))
    fullname = Column(String(50))


class AutoGraphMachineDriverRelation(BaseModel):
    __tablename__ = 'autograph_machine_driver_relations'

    driver_id = Column(Uuid, default=uuid.UUID(int=0))
    machine_id = Column(Uuid, default=uuid.UUID(int=0))


if __name__ == '__main__':
    Base.metadata.create_all(engine)
    # with Session() as session:
    #     demo_record = AutoGraphStage(
    #         id=uuid.UUID(int=0),
    #         stage='demo',
    #         datetime='some datetime - some datetime',
    #         machine_name='demo machine',
    #         machine_reg_number='demo reg number',
    #         benzo='0',
    #         driver_fullname='demo driver fullname'
    #     )
    #     session.add(demo_record)
    #     session.commit()
