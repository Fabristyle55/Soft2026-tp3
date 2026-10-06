using Dsw2025Ej15.Application.Exceptions;
using Dsw2025Tpi.Application.Dtos;
using Dsw2025Tpi.Domain.Entities;
using Dsw2025Tpi.Domain.Interfaces;

namespace Dsw2025Tpi.Application.Services;

public class OrderManagementService
{
    private readonly IRepository _repository;

    public OrderManagementService(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> CreateAsync(OrderModel.CreateRequest request)
    {
        var customer = await _repository.GetById<Customer>(request.CustomerId);
        if (customer == null)
            throw new NotFoundException("Cliente no encontrado.");

        var productIds = request.OrderItems.Select(i => i.ProductId).ToList();
        var products = await _repository.GetFiltered<Product>(p => productIds.Contains(p.Id))
                      ?? throw new BadRequestException("No se pudieron obtener los productos.");

        foreach (var item in request.OrderItems)
        {
            var product = products.FirstOrDefault(p => p.Id == item.ProductId);
            if (product == null)
                throw new NotFoundException($"Producto no encontrado: {item.Name}");

            if (product.stockQuantity < item.Quantity)
                throw new BadRequestException($"Stock insuficiente para '{item.Name}'. Disponible: {product.stockQuantity}");
        }

        var order = new Order
        {
            Id = Guid.NewGuid(),
            customerId = request.CustomerId,
            shippingAdress = request.ShippingAddress,
            billingAdress = request.BillingAddress,
            notes = request.Notes ?? "",
            status = OrderStatus.PENDIENTE,
            date = DateTime.UtcNow,
            items = request.OrderItems.Select(i => new OrderItem
            {
                Id = Guid.NewGuid(),
                productId = i.ProductId,
                name = i.Name,
                description = i.Description,
                unitPrice = i.UnitPrice,
                quantity = i.Quantity
            }).ToList()
        };

        foreach (var item in request.OrderItems)
        {
            var product = products.First(p => p.Id == item.ProductId);
            product.stockQuantity -= item.Quantity;
            await _repository.Update(product);
        }

        var result = await _repository.Add(order);
        return result.Id;
    }
}
