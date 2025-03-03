using menu.Interface;
using menu.Model;
using Microsoft.EntityFrameworkCore;

namespace menu.Repository
{
    public class DishRepository : GenericRepository<Dish>, IDishRepository
    {
        public DishRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Dish>> GetDishesByCategoryAsync(Guid categoryId)
        {
            return await _context.Dishes
                .Where(d => d.CategoryId == categoryId)
                .ToListAsync();
        }
    }
}
