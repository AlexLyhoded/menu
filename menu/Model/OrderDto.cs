﻿using System.Text.Json.Serialization;

namespace menu.Model
{
    public class OrderDto
    {
        public decimal? TotalAmount { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        public string Status { get; set; } = "В обробці"; // По умолчанию

        // Связь Many-to-Many с блюдами
        public List<string> Dishes { get; set; }
        public List<Guid>? DishesId { get; set; }
    }
}
