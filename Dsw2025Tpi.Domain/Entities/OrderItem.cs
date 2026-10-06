using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Domain.Entities;

public class OrderItem : EntityBase
{
    public Guid orderId { get; set; }
    public Order Order { get; set; }

    public Guid productId { get; set; }
    public Product Product { get; set; }
    public Guid Id { get; set; }
    public string name { get; set; }
    public string? description { get; set; }
    public int quantity { get; set; }
    public decimal unitPrice { get; set; }

    public decimal Subtotal => unitPrice * quantity;
}
