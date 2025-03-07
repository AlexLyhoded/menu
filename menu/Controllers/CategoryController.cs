using AutoMapper;
using menu.Interface;
using menu.Model;
using Microsoft.AspNetCore.Mvc;

namespace menu.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IDishRepository _dishRepository;

        public CategoryController(ICategoryRepository categoryRepository, IDishRepository dishRepository)
        {
            _categoryRepository = categoryRepository;
            _dishRepository = dishRepository;
        }
        // Добавить категорию
        public async Task<IActionResult> Create(Guid? id)
        {
            Category model;
            if (id.HasValue)
            {
                var category = await _categoryRepository.GetByIdAsync(id.Value);
                if (category == null)
                {
                    return NotFound();
                }
                model = category;
            }
            else
            {
                model = new Category();
            }
            ViewBag.DishesList = await _dishRepository.GetDistinctTitlesWithIdsAsync();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Create(Category category, List<Guid> Dishes, Guid? id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (id.HasValue)
            {
                var existingCategory = await _categoryRepository.GetByIdAsync(id.Value);
                if (existingCategory == null)
                {
                    return NotFound();
                }
                existingCategory.Title = category.Title;
                existingCategory.Description = category.Description;
                existingCategory.Dishes = Dishes ?? new List<Guid>(); // Сохраняем выбранные блюда

                await _categoryRepository.UpdateAsync(existingCategory);
            }
            else
            {
                category.Id = Guid.NewGuid();
                category.Dishes = Dishes ?? new List<Guid>(); // Сохраняем выбранные блюда
                await _categoryRepository.AddAsync(category);
            }

            return RedirectToAction("ManageCategories", "Admin");
        }
        // Удалить категорию
        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            // Убедитесь, что объект существует в базе данных
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            await _categoryRepository.DeleteAsync(id);
            return RedirectToAction("ManageCategories", "Admin");
        }
    }
}
