namespace Dsw2025Tpi.Application.Dtos;

public record OrderModel
{
    public record OrderItem(Guid ProductId, string Name, string? Description, decimal UnitPrice, int Quantity);

    public record CreateRequest(Guid CustomerId, string ShippingAddress, string BillingAddress, string? Notes, List<OrderItem> OrderItems);

    public record Response(Guid Id, Guid CustomerId, string ShippingAddress, string BillingAddress, string Notes,  DateTime Date, string Status, decimal TotalAmount, List<OrderItem> Items);
}
