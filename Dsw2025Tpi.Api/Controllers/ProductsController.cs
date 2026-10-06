using Dsw2025Ej15.Application.Services;
using Dsw2025Tpi.Application.Dtos;
using Microsoft.AspNetCore.Mvc;
using Dsw2025Ej15.Application.Exceptions;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Api.Controllers;


public class ProductsController : ControllerBase
{
    private readonly ProductManagementService _service;

    public ProductsController(ProductManagementService service)
    {
        _service = service;
    }

    //EndPoint #1 - Crear un producto
    [HttpPost]
    [Route("api/products")]
    public async Task<IActionResult> CreateProductAsync([FromBody] ProductModel.Request request)
    {
         var id = await _service.CreateAsync(request);
         return CreatedAtAction(nameof(_service.GetById), new { id }, new { id });
    }

    // Fix for CS1003 and CS1513 errors in the GetProducts method
    [HttpGet]
    [Route("api/products")]
    public IActionResult GetProducts()
    {
        var products = _service.GetAllAsync().Result; // Asume que el servicio devuelve una lista de productos
        if (products == null || !products.Any())
        {
            return NoContent();
        }
        return Ok(new { Message = "Lista de productos", Products = products });
    }

    //EndPoint #3 - Obtener un producto por ID
    [HttpGet]
    [Route("api/products/{id}")]
    public async Task<IActionResult> GetProductByIdAsync(Guid id)
    {
        var product = await _service.GetById(id);
        if (product == null)
            throw new NotFoundException("Producto no encontrado.");
            return Ok(product);

    }

    //EndPoint #4 - Actualizar un producto por ID
    [HttpPut]
    [Route("api/products/{id}")]
    public async Task<IActionResult> UpdateProductAsync(Guid id, [FromBody] ProductModel.Update product)
    {
        try
        {
            var updatedProduct = await _service.UpdateAsync(id, product);
            
            return Ok(updatedProduct);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (BadRequestException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            // Manejo genérico de errores
            return StatusCode(500, new { error = "Ocurrió un error inesperado.", details = ex.Message });
        }
    }

    //EndPoint #5 - Inhabilitar un producto por ID
    [HttpPatch]
    [Route("api/products/{id}")]
    public async Task<IActionResult> DisableProduct(Guid id)
    {
        try
        {
            await _service.DisableAsync(id);
            return NoContent();
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }
}
