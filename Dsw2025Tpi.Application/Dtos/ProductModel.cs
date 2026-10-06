using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Application.Dtos;

public record ProductModel
{
    public record Request(string sku, string internalCode, string name, string? description, decimal currentUnitPrice, int stockQuantity);

    public record Update(string sku, string internalCode, string name, string? description, decimal currentUnitPrice, int stockQuantity);

    public record Response(Guid Id, string sku, string internalCode, string name, string? description, decimal currentUnitPrice, int stockQuantity, bool isActive);
}
