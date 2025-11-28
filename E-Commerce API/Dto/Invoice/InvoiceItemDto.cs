namespace E_Commerce_API.Dto.Invoice
{
    public class InvoiceItemDto
    {
        public int ProductId { get; set; }
        public string ProductNameAr { get; set; }
        public string ProductNameEn { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Subtotal { get; set; }
    }
}
