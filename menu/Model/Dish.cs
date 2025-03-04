﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace menu.Model
{
    public class Dish
    {
        [JsonIgnore]
        public Guid Id { get; set; }

        [Required, MaxLength(100)]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public decimal Price { get; set; }
        [JsonIgnore]
        public byte[] Picture { get; set; }

        public bool IsAvailable { get; set; } = true;

        public double Weight { get; set; }
    }
}
