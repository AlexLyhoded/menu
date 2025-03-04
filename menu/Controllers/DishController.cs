using AutoMapper;
using menu.Interface;
using menu.Model;
using Microsoft.AspNetCore.Mvc;


namespace menu.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class DishController : ControllerBase
    {
        private readonly IDishRepository _dishRepository;
        private readonly IMapper _mapper;

        public DishController(IDishRepository dishRepository, IMapper mapper)
        {
            _dishRepository = dishRepository;
            _mapper = mapper;
        }

        // Получить все блюда
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var dishes = await _dishRepository.GetAllAsync();
            var dishDtos = _mapper.Map<List<DishDto>>(dishes); // Маппим список Dish в список DishDto
            return Ok(dishDtos);
        }

        // Получить одно блюдо
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var dish = await _dishRepository.GetByIdAsync(id);
            if (dish == null) return NotFound();
            var dishDto = _mapper.Map<DishDto>(dish); // Маппим один объект Dish в DishDto
            return Ok(dishDto);
        }

        // Добавить блюдо
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] DishDto dishDto, IFormFile file)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            byte[] pictureData = null;
            if (file != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    pictureData = memoryStream.ToArray();
                }
            }
            var dish = _mapper.Map<Dish>(dishDto); // Маппим DishDto в Dish

            dish.Id = Guid.NewGuid();
            dish.Picture = pictureData;

            await _dishRepository.AddAsync(dish);
            return CreatedAtAction(nameof(GetById), new { id = dish.Id }, dish);
        }

        // Обновить блюдо
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromForm] DishDto dishDto, IFormFile file)
        {
            var dish = await _dishRepository.GetByIdAsync(id);
            if (dish == null) return NotFound();
            _mapper.Map(dishDto, dish); // Маппим данные из DishDto в существующее блюдо

            if (file != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    dish.Picture = memoryStream.ToArray();
                }
            }

            await _dishRepository.UpdateAsync(dish);
            return NoContent();
        }
        // Удалить блюдо
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _dishRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
