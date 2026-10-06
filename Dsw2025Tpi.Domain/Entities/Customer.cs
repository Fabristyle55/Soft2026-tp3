
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Domain.Entities;

public class Customer : EntityBase
{
    public Customer()
    {
        Orders = new List<Order>();
    }

    public new Guid Id { get; set; }
    public string name { get; set; }
    public string email { get; set; }
    public string phoneNumber { get; set; }
     
    public ICollection<Order> Orders { get; set; }
}
