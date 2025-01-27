using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace mywebsite.Models.Products
{
    public class Keycaps
    {
        [Key] // Первинний ключ
        [ForeignKey("Product")] // Зв'язок із таблицею Product
        public int ProductId { get; set; }
        public Product Product { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Profile { get; set; }
        [Required]
        public string Material { get; set; }
        [Required]
        public string Sublegends { get; set; }
        [Required]
        public string ManufacturingProcess {  get; set; }
        [Required]
        public string LayoutStandard { get; set; }
        public string ImagePath { get; set; }
    }
}
