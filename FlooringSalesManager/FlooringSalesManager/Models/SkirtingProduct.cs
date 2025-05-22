namespace FlooringSalesManager.Models
{
    // For skirting boards (первази)
    public class SkirtingProduct : ProductBase
    {
        public decimal PricePerMeter { get; set; }
        public decimal LengthPerPiece { get; set; }
    }
}
