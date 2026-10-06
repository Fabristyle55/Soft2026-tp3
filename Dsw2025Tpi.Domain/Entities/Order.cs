using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Domain.Entities;

public class Order : EntityBase
{
    public Guid customerId { get; set; }
    public Customer customer { get; set; }
    public OrderStatus status { get; set; }

    public new Guid Id { get; set; }
    public DateTime date { get; set; }
    public string shippingAdress { get; set; }
    public string billingAdress { get; set; }
    public string? notes { get; set; }
    public decimal totalAmount => items.Sum(item => item.unitPrice * item.quantity);

    public ICollection<OrderItem> items { get; set; }
}
