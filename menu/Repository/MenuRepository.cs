using menu.Interface;
using menu.Model;
using Microsoft.EntityFrameworkCore;

namespace menu.Repository
{
    public class MenuRepository : GenericRepository<Menu>, IMenuRepository
    {
        public MenuRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Menu>> GetActiveMenusAsync()
        {
            return await _context.Menus.Where(m => m.IsActive).ToListAsync();
        }
    }
}
