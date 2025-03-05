using menu.Extensions;
using menu.Interface;
using menu.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc; // Для работы с сессией

public class OrderController : Controller
{
    private readonly IOrderRepository _orderRepository;
    private readonly IDishRepository _dishRepository;

    public OrderController(IOrderRepository orderRepository, IDishRepository dishRepository)
    {
        _orderRepository = orderRepository;
        _dishRepository = dishRepository;
    }

    // Метод для добавления блюда в заказ
    [HttpPost]
    public async Task<IActionResult> AddToOrder(Guid dishId)
    {
        var userId = HttpContext.Session.GetString("UserId") ?? Guid.NewGuid().ToString();
        HttpContext.Session.SetString("UserId", userId);

        // Ищем незавершённый заказ пользователя
        var order = await _orderRepository.GetActiveOrderByUserIdAsync(userId);

        if (order == null)
        {
            // Создаём новый заказ
            order = new Order
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                IsCompleted = false,
                Dishes = new List<Guid>(),
                TotalAmount = 0
            };
            await _orderRepository.AddAsync(order);
        }

        // Добавляем блюдо
        var dish = await _dishRepository.GetByIdAsync(dishId);
        if (dish == null) return NotFound("Блюдо не найдено");

        order.Dishes.Add(dish.Id);
        order.TotalAmount += dish.Price;

        await _orderRepository.UpdateAsync(order);

        return RedirectToAction("Index", "Home");
    }


    // Просмотр заказа
    public IActionResult ViewOrder()
    {
        var order = HttpContext.Session.GetObject<Order>("CurrentOrder");
        return View(order);
    }

    // Завершение заказа
    [HttpPost]
    public async Task<IActionResult> CompleteOrder()
    {
        var order = HttpContext.Session.GetObject<Order>("CurrentOrder");
        if (order == null || !order.Dishes.Any())
        {
            return BadRequest("Замовлення порожнє");
        }

        // Сохраняем заказ в БД
        await _orderRepository.AddAsync(order);

        // Очищаем сессию
        HttpContext.Session.Remove("CurrentOrder");

        return RedirectToAction("OrderSuccess");
    }

    public IActionResult OrderSuccess()
    {
        return View();
    }
}
