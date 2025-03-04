﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace menu.Model
{
    public class Order
    {
        [JsonIgnore]
        public Guid Id { get; set; }
        [JsonIgnore]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        public string Status { get; set; } = "В обробці"; 

        public List<Guid> Dishes { get; set; } = new List<Guid>();

    }
}
