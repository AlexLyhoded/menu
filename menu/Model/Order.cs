using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace menu.Model
{
    public class Order
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        public string Status { get; set; } = "В обробці"; 

        public List<Dish> Dishes { get; set; }
        public List<Guid> DishesId { get; set; }
        public bool IsCompleted { get; set; }

    }
}
