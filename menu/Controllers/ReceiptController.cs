using menu.Interface;
using menu.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace menu.Controllers
{
    public class ReceiptController : Controller
    {
        private readonly IReceiptRepository _receiptRepository;
        private readonly IOrderRepository _orderRepository;

        public ReceiptController(IReceiptRepository receiptRepository, IOrderRepository orderRepository)
        {
            _receiptRepository = receiptRepository;
            _orderRepository = orderRepository;
        }

        // Відображення сторінки оплати з замовленням
        [HttpGet("payment/{orderId}")]
        public async Task<IActionResult> Payment(Guid orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null)
            {
                return NotFound("Замовлення не знайдено");
            }

            // Надсилаємо замовлення на сторінку для відображення
            return View(order);
        }

        // Створення чека з вибраним методом оплати
        [HttpPost("create-receipt")]
        public async Task<IActionResult> CreateReceipt(Guid orderId, string paymentMethod)
        {
            // Отримуємо замовлення по ID
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null)
            {
                return NotFound("Замовлення не знайдено");
            }

            // Створюємо новий чек
            var receipt = new Receipt
            {
                Id = Guid.NewGuid(),
                OrderId = order.Id,
                TotalAmount = order.TotalAmount,
                PaymentMethod = paymentMethod,
                Date = DateTime.UtcNow,
                Status = "Оплачено"
            };

            // Зберігаємо чек
            await _receiptRepository.AddAsync(receipt);

            // Перенаправляємо користувача на сторінку успішної оплати
            return RedirectToAction("PaymentSuccess", new { receiptId = receipt.Id });
        }

        // Сторінка успішної оплати
        [HttpGet("payment-success/{receiptId}")]
        public async Task<IActionResult> PaymentSuccess(Guid receiptId)
        {
            var receipt = await _receiptRepository.GetByIdAsync(receiptId);
            if (receipt == null)
            {
                return NotFound("Чек не знайдено");
            }

            return View(receipt); // Відправляємо чек на сторінку для відображення
        }
    }
}
