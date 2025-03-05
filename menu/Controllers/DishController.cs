using AutoMapper;
using menu.Interface;
using menu.Model;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;


namespace menu.Controllers
{
    public class DishController : Controller
    {
        private readonly IDishRepository _dishRepository;
        private readonly IMapper _mapper;

        public DishController(IDishRepository dishRepository, IMapper mapper)
        {
            _dishRepository = dishRepository;
            _mapper = mapper;
        }
        public async Task<IActionResult> Create(Guid? id)
        {
            Dish model;
            if (id.HasValue)
            {
                var dish = await _dishRepository.GetByIdAsync(id.Value);
                if (dish == null) return NotFound();
                model = dish;
            }
            else
            {
                model = new Dish();
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Dish dish, IFormFile? file, Guid? id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            byte[] pictureData = null;

            // Если файл был передан, загружаем его
            if (file != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    pictureData = memoryStream.ToArray();
                }
            }

            if (id.HasValue)
            {
                // Получаем существующее блюдо
                var existingDish = await _dishRepository.GetByIdAsync(id.Value);
                if (existingDish == null)
                {
                    return NotFound();
                }
                existingDish.Title = dish.Title;
                existingDish.Price = dish.Price;
                existingDish.Description = dish.Description;
                existingDish.IsAvailable = dish.IsAvailable;
                existingDish.Weight = dish.Weight;

                // Если фото новое, обновляем его
                if (file != null)
                {
                    existingDish.Picture = pictureData;
                }

                    // Сохраняем обновления
                    await _dishRepository.UpdateAsync(existingDish); // Обновляем существующее блюдо
            }
            else
            {
                // Генерируем новый Id и устанавливаем фото, если оно передано
                dish.Id = Guid.NewGuid();
                dish.Picture = pictureData;

                // Сохраняем новое блюдо
                await _dishRepository.AddAsync(dish); // Добавляем новое блюдо
            }

            return RedirectToAction("ManageDishes", "Admin");
        }

        // Удалить блюдо
        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            // Убедитесь, что объект существует в базе данных
            var dish = await _dishRepository.GetByIdAsync(id);
            if (dish == null)
            {
                return NotFound();
            }

            await _dishRepository.DeleteAsync(id);
            return RedirectToAction("ManageDishes", "Admin");
        }

    }
}
