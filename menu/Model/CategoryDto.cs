using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace menu.Model
{
    public class CategoryDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> Dishes { get; set; }
        [JsonIgnore]
        public List<Guid>? DishesId { get; set; }
    }
}
