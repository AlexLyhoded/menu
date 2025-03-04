using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace menu.Model
{
    public class Receipt
    {
        [JsonIgnore]
        public Guid Id { get; set; }

        [Required]
        public Guid OrderId { get; set; }  // Привязка к заказу

        [Column(TypeName = "decimal(18,2)")]
        [JsonIgnore]
        public decimal? TotalAmount { get; set; }  // Сумма

        [Required]
        public string PaymentMethod { get; set; }  // "Готівка", "Карта"

        public DateTime Date { get; set; } = DateTime.UtcNow; // Дата

        public string Status { get; set; } = "Оплачено";  // Или "Не оплачено"
    }
}
