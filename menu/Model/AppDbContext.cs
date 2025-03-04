using Microsoft.EntityFrameworkCore;

namespace menu.Model
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Dish> Dishes { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Receipt> Receipts { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Dish>()
                .Property(d => d.Price)
                .HasColumnType("decimal(10,2)");
        }
    }
}
