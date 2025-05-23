namespace FlooringSalesManager.Models
{
    public class OrderItem
    {
        public int OrderItemId { get; set; }
        public int OrderId { get; set; }
        public string ProductType { get; set; } = ""; // "Flooring" or "Skirting"
        public string ProductNumber { get; set; } = "";
        public double Quantity { get; set; }      // m2 for flooring, meters for skirting
        public decimal UnitPrice { get; set; }    // set per-order, not tied to Product DB
    }
}
