namespace FlooringSalesManager.Models
{
    // For flooring products like laminate or SPC
    public class FlooringProduct : ProductBase
    {
        public decimal PricePerSquareMeter { get; set; }
        public decimal M2PerBox { get; set; }
    }
}
