using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Domain.Entities;

public class Product : EntityBase
{
    public object orders;

    public Product()
    {

    }

    public Product(string sku, string internalCode, string name, string description, decimal currentUnitPrice, int stockQuantity, bool isActive)
    {
        this.sku = sku;
        this.internalCode = internalCode;
        this.name = name;
        this.description = description;
        this.currentUnitPrice = currentUnitPrice;
        this.stockQuantity = stockQuantity;
        this.isActive = isActive;
    }

    public new Guid Id { get; set; }
    public string sku { get; set; }
    public string? internalCode { get; set; }
    public string name { get; set; }
    public string? description { get; set; }
    public decimal currentUnitPrice { get; set; }
    public int stockQuantity { get; set; }
    public bool isActive { get; set; }


}
