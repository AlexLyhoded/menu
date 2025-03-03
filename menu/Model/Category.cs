using System.ComponentModel.DataAnnotations;

namespace menu.Model
{
    public class Category
    {
        [Key]
        public Guid Id { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; }

        public string Description { get; set; }

        public List<Guid> Dishes { get; set; }
    }
}
