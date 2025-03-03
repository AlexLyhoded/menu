using menu.Model;

namespace menu.Interface
{
    public interface IMenuRepository : IGenericRepository<Menu>
    {
        Task<IEnumerable<Menu>> GetActiveMenusAsync();
    }
}
