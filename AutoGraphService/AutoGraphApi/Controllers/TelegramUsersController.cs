using AutoGraphApi.Models;
using AutoGraphApi.Services;

namespace AutoGraphApi.Controllers;

public class TelegramUsersController(IGenericService<AutoGraphUserEntity> service)
    : GenericController<AutoGraphUserEntity>(service);