using Microsoft.AspNetCore.Mvc;
using Dsw2025Tpi.Application.Services;
using Dsw2025Tpi.Application.Dtos;

namespace Dsw2025Tpi.API.Controllers;

[ApiController]
[Route("api/orders")]
public class OrdersController : ControllerBase
{
    private readonly OrderManagementService _service;

    public OrdersController(OrderManagementService service)
    {
        _service = service;
    }

    // EndPoint #6 - Crear una Orden
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] OrderModel.CreateRequest request)
    {
        var id = await _service.CreateAsync(request);
        return CreatedAtAction(nameof(GetByIdPlaceholder), new { id }, new { id });
    }

    // Solo evita que CreatedAtAction rompa y se reemplaza SOLO cuando se tenga el GetById real implementado
    [HttpGet("{id:guid}")]
    public IActionResult GetByIdPlaceholder(Guid id) => Ok(new { id });
}
