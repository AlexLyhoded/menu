using menu.Model;
using menu.Interface;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace menu.Pages.Admin
{
    public class ManageDishesModel : PageModel
    {
        private readonly IDishRepository _dishRepository;

        public ManageDishesModel(IDishRepository dishRepository)
        {
            _dishRepository = dishRepository;
        }

        public List<Dish> Dishes { get; set; }

        public async Task OnGetAsync()
        {
            // Получаем все блюда
            Dishes = (List<Dish>)await _dishRepository.GetAllAsync();
        }
    }
}
