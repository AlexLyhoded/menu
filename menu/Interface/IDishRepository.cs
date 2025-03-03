using menu.Model;

namespace menu.Interface
{
    public interface IDishRepository : IGenericRepository<Dish>
    {
        Task<IEnumerable<Dish>> GetDishesByCategoryAsync(Guid categoryId);
    }
}
