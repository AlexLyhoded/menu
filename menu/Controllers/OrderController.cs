using AutoMapper;
using menu.Interface;
using menu.Model;
using menu.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

[Route("api/orders")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly IOrderRepository _orderRepository;
    private readonly IDishRepository _dishRepository;
    private readonly IMapper _mapper;

    public OrderController(IOrderRepository orderRepository, IMapper mapper, IDishRepository dishRepository)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
        _dishRepository = dishRepository;
    }

    // Отримати всі замовлення
    [HttpGet]
    public async Task<IActionResult> GetAllOrders()
    {
        var orders = await _orderRepository.GetAllAsync();
        var ordersDto = _mapper.Map<IEnumerable<OrderDto>>(orders);
        return Ok(ordersDto);
    }

    // Отримати замовлення за ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrderById(Guid id)
    {
        var order = await _orderRepository.GetByIdAsync(id);
        if (order == null)
            return NotFound("Замовлення не знайдено");

        return Ok(_mapper.Map<OrderDto>(order));
    }

    // Створити нове замовлення
    [HttpPost]
    public async Task<ActionResult> CreateOrder([FromForm] OrderDto orderDto)
    {
        if (orderDto == null)
            return BadRequest("Некоректні дані");
        List<Guid> dishes = new List<Guid>();
        decimal price = 0;
        foreach (var dishTitle in orderDto.Dishes)
        {
            var dish = await _dishRepository.GetByNameAsync(dishTitle);
            if (dish != null)
            {
                dishes.Add(dish.Id);
                price += dish.Price;
            }
            else
            {
                // Если блюдо не найдено, возвращаем ошибку
                return BadRequest($"Dish with title '{dishTitle}' not found.");
            }
        }
        orderDto.DishesId = dishes;
        orderDto.TotalAmount = price;
        var order = _mapper.Map<Order>(orderDto);
        await _orderRepository.AddAsync(order);

        return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, _mapper.Map<OrderDto>(order));
    }

    // Оновити замовлення
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateOrder(Guid id, [FromForm] OrderDto orderDto)
    {
        if (orderDto == null)
            return BadRequest("Некоректні дані");

        var existingOrder = await _orderRepository.GetByIdAsync(id);
        if (existingOrder == null)
            return NotFound("Замовлення не знайдено");
        if (orderDto.Dishes != null)
        {
            // Загружаем все блюда по их названиям
            List<Guid> dishes = new List<Guid>();
            decimal price = 0;
            foreach (var dishTitle in orderDto.Dishes)
            {
                var dish = await _dishRepository.GetByNameAsync(dishTitle);
                if (dish != null)
                {
                    dishes.Add(dish.Id);
                    price += dish.Price;
                }
                else
                {
                    // Если блюдо не найдено, можно обработать это как ошибку
                    return BadRequest($"Dish with title '{dishTitle}' not found.");
                }
            }
            orderDto.DishesId = dishes;
            orderDto.TotalAmount = price;
        }
        _mapper.Map(orderDto, existingOrder);
        await _orderRepository.UpdateAsync(existingOrder);

        return NoContent();
    }

    // Видалити замовлення
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteOrder(Guid id)
    {
        var existingOrder = await _orderRepository.GetByIdAsync(id);
        if (existingOrder == null)
            return NotFound("Замовлення не знайдено");

        await _orderRepository.DeleteAsync(id);
        return NoContent();
    }
}
