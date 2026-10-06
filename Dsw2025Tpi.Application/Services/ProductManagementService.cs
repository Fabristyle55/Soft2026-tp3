using Dsw2025Tpi.Application.Dtos;
using Dsw2025Tpi.Domain.Entities;
using Dsw2025Tpi.Domain.Interfaces;
using Dsw2025Ej15.Application.Exceptions;

namespace Dsw2025Ej15.Application.Services;

public class ProductManagementService
{
    private readonly IRepository _repository;

    public ProductManagementService(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> CreateAsync(ProductModel.Request request)
    {
        if (request is null)
            throw new BadRequestException("El cuerpo de la solicitud no puede estar vacío o mal formado.");

        var exists = await _repository.First<Product>(p => p.sku == request.sku);
        if (exists != null)
            throw new BadRequestException($"Ya existe un producto con el SKU '{request.sku}'.");

        if (request.stockQuantity <= 0)
            throw new BadRequestException("El stock debe ser mayor a cero.");

        if (request.currentUnitPrice <= 0)
            throw new BadRequestException("El precio debe ser mayor a cero.");

        var product = new Product
        {
            sku = request.sku,
            name = request.name,
            currentUnitPrice = request.currentUnitPrice,
            stockQuantity = request.stockQuantity,
            internalCode = request.internalCode,
            description = request.description,
            isActive = true
        };

        var result = await _repository.Add(product);
        return result.Id;
    }

    public async Task<ProductModel.Response?> GetById(Guid id)
    {
        var product = await _repository.GetById<Product>(id);
        if (product == null) 
            throw new NotFoundException("Producto no encontrado.");

        return new ProductModel.Response(
            product.Id,
            product.sku,
            product.internalCode,
            product.name,
            product.description,
            product.currentUnitPrice,
            product.stockQuantity, 
            product.isActive
        );
    }

    public async Task<IEnumerable<ProductModel.Response>> GetAllAsync()
    {
        var products = await _repository.GetAll<Product>() ?? Enumerable.Empty<Product>();

        return products.Select(p => new ProductModel.Response(p.Id, p.sku, p.internalCode, p.name, p.description, p.currentUnitPrice, p.stockQuantity, p.isActive));
    }

    public async Task<ProductModel.Response> UpdateAsync(Guid id, ProductModel.Update request)
    {
        if (request is null)
            throw new BadRequestException("El cuerpo de la solicitud no puede estar vacío o mal formado.");

        var product = await _repository.GetById<Product>(id);
        if (product == null ||
            string.IsNullOrWhiteSpace(request.sku) ||
            string.IsNullOrWhiteSpace(request.internalCode) ||
            string.IsNullOrWhiteSpace(request.name) ||
            request.currentUnitPrice <= 0)
            throw new NotFoundException("Producto no encontrado.");

        product.sku = request.sku;
         product.name = request.name;
         product.currentUnitPrice = request.currentUnitPrice;
         product.stockQuantity = request.stockQuantity;
         product.internalCode = request.internalCode;
         product.description = request.description;
        product.isActive = true;

        var updated = await _repository.Update(product);

        return new ProductModel.Response(
            product.Id,
            product.sku,
            product.internalCode,
            product.name,
            product.description,
            product.currentUnitPrice,
            product.stockQuantity,
            product.isActive
        );
    }

    public async Task DisableAsync(Guid id)
    {
        var product = await _repository.GetById<Product>(id);
        if (product == null)
            throw new NotFoundException("Producto no encontrado.");

        product.isActive = false;
        await _repository.Update(product);
    }

    public async Task UpdateAsync(Guid id, object product)
    {
        throw new NotImplementedException();
    }
}
