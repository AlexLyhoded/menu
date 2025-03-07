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
    public class PaymentModel : PageModel
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IReceiptRepository _receiptRepository;
        private readonly IDishRepository _dishRepository; // ✅ Добавляем репозиторий для загрузки блюд

        public PaymentModel(IOrderRepository orderRepository, IReceiptRepository receiptRepository, IDishRepository dishRepository)
        {
            _orderRepository = orderRepository;
            _receiptRepository = receiptRepository;
            _dishRepository = dishRepository;
        }

        public Order Order { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid orderId)
        {
            Order = await _orderRepository.GetByIdAsync(orderId);
            if (Order == null)
            {
                return NotFound("Замовлення не знайдено");
            }

            // ✅ Загружаем блюда по их ID из DishesId
            var dishes = new List<Dish>();
            foreach (var dishId in Order.DishesId)
            {
                var dish = await _dishRepository.GetByIdAsync(dishId);
                if (dish != null)
                {
                    dishes.Add(dish);
                }
            }

            Order.Dishes = dishes; // Записываем найденные блюда в заказ
            return Page();
        }

        public async Task<IActionResult> OnPostCreateReceiptAsync(Guid orderId, string paymentMethod)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null)
            {
                return NotFound("Замовлення не знайдено");
            }

            // Создаём чек
            var receipt = new Receipt
            {
                Id = Guid.NewGuid(),
                OrderId = order.Id,
                TotalAmount = order.TotalAmount,
                PaymentMethod = paymentMethod,
                Date = DateTime.UtcNow,
                Status = "Сплачено"
            };

            await _receiptRepository.AddAsync(receipt);

            order.IsCompleted = true;
            order.Status = "Завершено";
            await _orderRepository.UpdateAsync(order);

            return RedirectToPage("PaymentSuccess", new { receiptId = receipt.Id });
        }

    }
}
