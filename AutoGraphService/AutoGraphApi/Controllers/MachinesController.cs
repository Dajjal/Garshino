using AutoGraphApi.Models;
using AutoGraphApi.Services;

namespace AutoGraphApi.Controllers;

public class MachinesController(IGenericService<AutoGraphMachinesEntity> service)
    : GenericController<AutoGraphMachinesEntity>(service);