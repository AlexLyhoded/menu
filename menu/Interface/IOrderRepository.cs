using menu.Model;

namespace menu.Interface
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<Order?> GetActiveOrderByUserIdAsync(string userId);
    }
}
