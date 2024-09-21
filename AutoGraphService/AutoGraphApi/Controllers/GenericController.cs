using AutoGraphApi.Models;
using AutoGraphApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace AutoGraphApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GenericController<TEntity>(IGenericService<TEntity> service) : ControllerBase
    where TEntity : AbstractEntity
{
    [HttpGet]
    public async Task<IActionResult> ListAsync(
        CancellationToken cancellationToken = default
    )
    {
        var response = await service.ListAsync(cancellationToken);
        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetByIdAsync(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default
    )
    {
        var response = await service.GetByIdAsync(id, cancellationToken);
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> AddAsync(
        [FromBody] TEntity dto,
        CancellationToken cancellationToken = default
    )
    {
        var response = await service.AddAsync(dto, cancellationToken);
        return Ok(response);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAsync(
        [FromRoute] Guid id,
        [FromBody] TEntity dto,
        CancellationToken cancellationToken = default)
    {
        var response = await service.UpdateAsync(id, dto, cancellationToken);
        return Ok(response);
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default
    )
    {
        var response = await service.DeleteAsync(id, cancellationToken);
        return Ok(response);
    }
}