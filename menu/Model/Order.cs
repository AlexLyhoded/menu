using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace menu.Model
{
    public class Order
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        public string Status { get; set; } = "В обробці"; // По умолчанию

        // Связь Many-to-Many с блюдами
        public List<Guid> Dishes { get; set; } = new List<Guid>();

    }
}
