namespace E_Commerce_API.Dto.Invoice
{
    public class InvoiceResponseDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public decimal TotalAmount { get; set; }
        public List<InvoiceItemDto> Items { get; set; }
    }
}
