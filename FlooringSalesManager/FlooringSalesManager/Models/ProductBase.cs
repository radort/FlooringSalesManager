namespace FlooringSalesManager.Models
{
    // Abstract base class for all product types
    public abstract class ProductBase
    {
        public int Id { get; set; }
        public string Number { get; set; } = "";
        public string Name { get; set; } = "";
        public string Type { get; set; } = ""; 
    }
}
