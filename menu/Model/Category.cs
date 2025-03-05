using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace menu.Model
{
    public class Category
    {
        public Guid Id { get; set; }

        [Required, MaxLength(50)]
        public string Title { get; set; }

        public string Description { get; set; }
        public List<Guid> Dishes { get; set; } = new List<Guid>();
    }
}
