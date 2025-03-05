using menu.Interface;
using menu.Model;
using Microsoft.EntityFrameworkCore;

namespace menu.Repository
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(AppDbContext context) : base(context) { }
        public async Task<Order?> GetActiveOrderByUserIdAsync(string userId)
        {
            return await _context.Orders
                .Include(o => o.Dishes)
                .FirstOrDefaultAsync(o => o.UserId == userId && !o.IsCompleted);
        }

    }
}
