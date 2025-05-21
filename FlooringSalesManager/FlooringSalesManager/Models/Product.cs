using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlooringSalesManager.Models
{
    class Product
    {
            public int Id { get; set; }
            public string Number { get; set; } = "";
            public string Name { get; set; } = "";
            public string Type { get; set; } = "";
            public decimal PricePerSquareMeter { get; set; }         
            public decimal M2PerBox { get; set; }         
    }
}
