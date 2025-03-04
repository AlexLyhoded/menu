using AutoMapper;
using menu.Interface;
using menu.Model;
using Microsoft.AspNetCore.Mvc;

namespace menu.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly IDishRepository _dishRepository;

        public CategoryController(ICategoryRepository categoryRepository, IMapper mapper, IDishRepository dishRepository)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _dishRepository = dishRepository;
        }

        // Получить все категории
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _categoryRepository.GetAllAsync();
            var categoryDtos = _mapper.Map<IEnumerable<CategoryDto>>(categories);
            return Ok(categoryDtos);
        }

        // Получить одну категорию
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null) return NotFound();
            var categoryDto = _mapper.Map<CategoryDto>(category);
            return Ok(categoryDto);
        }

        // Добавить категорию
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CategoryDto categoryDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (categoryDto.Dishes == null || !categoryDto.Dishes.Any())
            {
                return BadRequest("Dishes cannot be null or empty.");
            }

            // Загружаем все блюда по их названиям
            List<Guid> dishes = new List<Guid>();
            foreach (var dishTitle in categoryDto.Dishes)
            {
                var dish = await _dishRepository.GetByNameAsync(dishTitle);
                if (dish != null)
                {
                    dishes.Add(dish.Id);
                }
                else
                {
                    // Если блюдо не найдено, возвращаем ошибку
                    return BadRequest($"Dish with title '{dishTitle}' not found.");
                }
            }

            // Маппим DTO в сущность для создания категории
            categoryDto.DishesId = dishes;
            var category = _mapper.Map<Category>(categoryDto);
            category.Id = Guid.NewGuid();
            await _categoryRepository.AddAsync(category);

            return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromForm] CategoryDto categoryDto)
        {
            var existingCategory = await _categoryRepository.GetByIdAsync(id);
            if (existingCategory == null) return NotFound();

            if (categoryDto.Dishes != null)
            {
                // Загружаем все блюда по их названиям
                List<Guid> dishes = new List<Guid>();
                foreach (var dishTitle in categoryDto.Dishes)
                {
                    var dish = await _dishRepository.GetByNameAsync(dishTitle);
                    if (dish != null)
                    {
                        dishes.Add(dish.Id);
                    }
                    else
                    {
                        // Если блюдо не найдено, можно обработать это как ошибку
                        return BadRequest($"Dish with title '{dishTitle}' not found.");
                    }
                }
                categoryDto.DishesId = dishes;
            }
            
            // Маппим DTO в сущность для обновления
            _mapper.Map(categoryDto, existingCategory);

            await _categoryRepository.UpdateAsync(existingCategory);

            return NoContent();
        }


        // Удалить категорию
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _categoryRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
