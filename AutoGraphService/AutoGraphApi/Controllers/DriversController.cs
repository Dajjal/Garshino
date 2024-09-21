using AutoGraphApi.Models;
using AutoGraphApi.Services;

namespace AutoGraphApi.Controllers;

public class DriversController(IGenericService<AutoGraphDriverEntity> service)
    : GenericController<AutoGraphDriverEntity>(service);