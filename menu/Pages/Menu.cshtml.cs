using menu.Interface;
using menu.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace menu.Pages
{
    public class MenuPageModel : PageModel
    {
        private readonly IDishRepository _dishRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IOrderRepository _orderRepository;

        public List<Dish> Dishes { get; set; }
        public List<Category> Categories { get; set; }
        public Guid? SelectedCategory { get; set; }
        public Guid? OrderId { get; set; }  // Добавляем OrderId в модель!

        public MenuPageModel(IDishRepository dishRepository, ICategoryRepository categoryRepository, IOrderRepository orderRepository)
        {
            _dishRepository = dishRepository;
            _categoryRepository = categoryRepository;
            _orderRepository = orderRepository;
        }

        public async Task OnGet(Guid? categoryId)
        {
            Categories = (await _categoryRepository.GetAllAsync()).ToList();
            SelectedCategory = categoryId;
            Dishes = new List<Dish>();

            if (categoryId.HasValue)
            {
                var category = await _categoryRepository.GetByIdAsync(categoryId.Value);
                if (category != null)
                {
                    foreach (var id in category.Dishes)
                    {
                        var dish = await _dishRepository.GetByIdAsync(id);
                        if (dish != null)
                        {
                            Dishes.Add(dish);
                        }
                    }
                }
            }
            else
            {
                Dishes = (await _dishRepository.GetAllAsync()).ToList();
            }
            // Получаем активный заказ пользователя
            var userId = HttpContext.Session.GetString("UserId");
            if (!string.IsNullOrEmpty(userId))
            {
                var order = await _orderRepository.GetActiveOrderByUserIdAsync(userId);
                if (order != null)
                {
                    OrderId = order.Id;  // Сохраняем orderId в модель
                }
            }
        }

        public async Task<IActionResult> OnPostAsync(Guid dishId)
        {
            var userId = HttpContext.Session.GetString("UserId") ?? Guid.NewGuid().ToString();
            HttpContext.Session.SetString("UserId", userId);

            var order = await _orderRepository.GetActiveOrderByUserIdAsync(userId);

            if (order == null)
            {
                order = new Order
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    IsCompleted = false,
                    Dishes = new List<Dish>(),
                    DishesId = new List<Guid>(),
                    TotalAmount = 0
                };
                await _orderRepository.AddAsync(order);
            }

            var dish = await _dishRepository.GetByIdAsync(dishId);
            if (dish == null) return NotFound("Блюдо не найдено");

            order.Dishes.Add(dish);
            order.DishesId.Add(dishId);
            order.TotalAmount += dish.Price;

            await _orderRepository.UpdateAsync(order);

            return RedirectToPage("./Menu");
        }
    }
}
