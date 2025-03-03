using System.ComponentModel.DataAnnotations;

namespace menu.Model
{
    public class Menu
    {
        [Key]
        public Guid Id { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; }

        public string Description { get; set; }

        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        public bool IsActive { get; set; }

        public List<Guid> Dishes { get; set; }
    }
}
