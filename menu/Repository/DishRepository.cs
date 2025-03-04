using menu.Interface;
using menu.Model;
using Microsoft.EntityFrameworkCore;

namespace menu.Repository
{
    public class DishRepository : GenericRepository<Dish>, IDishRepository
    {
        public DishRepository(AppDbContext context) : base(context) { }
    }
}
