using menu.Interface;
using menu.Model;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace menu.Repository
{
    public class DishRepository : GenericRepository<Dish>, IDishRepository
    {
        public DishRepository(AppDbContext context) : base(context) { }

        public async Task<List<SelectListItem>> GetDistinctTitlesWithIdsAsync()
        {
            return await _context.Dishes
                    .Select(x => new SelectListItem
                    {
                        Value = x.Id.ToString(),
                        Text = x.Title
                    })
                    .Distinct()
                    .ToListAsync();
        }
    }
}
