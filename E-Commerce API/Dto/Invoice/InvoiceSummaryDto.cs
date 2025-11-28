namespace E_Commerce_API.Dto.Invoice
{
    public class InvoiceSummaryDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string UserName { get; set; }
        public decimal TotalAmount { get; set; }
        public int ItemsCount { get; set; }
    }
}
