using menu.Interface;
using menu.Model;

namespace menu.Repository
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(AppDbContext context) : base(context) { }
    }
}
