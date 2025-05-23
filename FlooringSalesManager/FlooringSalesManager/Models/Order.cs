using System;
using System.Collections.Generic;

namespace FlooringSalesManager.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string CustomerName { get; set; } = "";
        public string CustomerPhone { get; set; } = "";
        public List<OrderItem> Items { get; set; } = new();
    }
}