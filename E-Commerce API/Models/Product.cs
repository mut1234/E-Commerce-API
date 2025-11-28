using System.ComponentModel.DataAnnotations;

namespace E_Commerce_API.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public decimal Price { get; set; }
        public bool IsDeleted { get; set; }

    }
}
