using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace mywebsite.Models.Products
{
    public class Switch
    {
        [Key] // Первинний ключ
        [ForeignKey("Product")] // Зв'язок із таблицею Product
        public int ProductId { get; set; }
        public Product Product { get; set; }
        [Required] 
        public string Name { get; set; }

        [Required] 
        public string Type { get; set; }

        [Required] 
        public int OperatingForce { get; set; }

        [Required] 
        public int TotalTravel { get; set; }

        [Required] 
        public int PreTravel { get; set; }

        public int TactilePosition { get; set; }
        public int TactileForce { get; set; }
        [Required]
        public string ImagePath { get; set; } = "imagehere.png";

    }
}
