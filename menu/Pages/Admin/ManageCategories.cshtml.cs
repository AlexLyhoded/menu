using menu.Interface;
using menu.Model;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace menu.Pages.Admin
{
    public class ManageCategoriesModel : PageModel
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IDishRepository _dishRepository;

        public ManageCategoriesModel(ICategoryRepository categoryRepository, IDishRepository dishRepository)
        {
            _categoryRepository = categoryRepository;
            _dishRepository = dishRepository;
        }

        public List<CategoryViewModel> Categories { get; set; }

        public async Task OnGetAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            Categories = new List<CategoryViewModel>();

            foreach (var category in categories)
            {
                var dishTitles = new List<string>();

                foreach (var dishId in category.Dishes) // category.Dishes хранит список ID блюд
                {
                    var dish = await _dishRepository.GetByIdAsync(dishId);
                    if (dish != null)
                    {
                        dishTitles.Add(dish.Title);
                    }
                }

                Categories.Add(new CategoryViewModel
                {
                    Id = category.Id,
                    Title = category.Title,
                    Description = category.Description,
                    Dishes = dishTitles
                });
            }
        }
    }

    public class CategoryViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<string> Dishes { get; set; }
    }
}
