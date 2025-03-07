using menu.Model;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace menu.Interface
{
    public interface IDishRepository : IGenericRepository<Dish>
    {
        Task<List<SelectListItem>> GetDistinctTitlesWithIdsAsync();
    }
}
