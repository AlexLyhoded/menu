using menu.Interface;
using menu.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace menu.Pages
{
    public class PaymentModel : PageModel
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IReceiptRepository _receiptRepository;
        private readonly IDishRepository _dishRepository;

        // 🔐 Публічний та приватний ключі LiqPay
        private readonly string _publicKey;
        private readonly string _privateKey;

        public PaymentModel(
            IOrderRepository orderRepository,
            IReceiptRepository receiptRepository,
            IDishRepository dishRepository,
            IConfiguration configuration)
        {
            _orderRepository = orderRepository;
            _receiptRepository = receiptRepository;
            _dishRepository = dishRepository;

            _publicKey = configuration["LiqPay:PublicKey"];
            _privateKey = configuration["LiqPay:PrivateKey"];
        }


        // 🔄 Властивості для передачі на сторінку
        public Order Order { get; set; }
        public string LiqPayData { get; set; }
        public string LiqPaySignature { get; set; }

        // ⏳ Метод GET: завантажуємо замовлення і генеруємо дані для LiqPay
        public async Task<IActionResult> OnGetAsync(Guid orderId)
        {
            Order = await _orderRepository.GetByIdAsync(orderId);
            if (Order == null)
                return NotFound("Замовлення не знайдено");

            // ⬇️ Завантажуємо страви по їхніх ID
            var dishes = new List<Dish>();
            foreach (var dishId in Order.DishesId)
            {
                var dish = await _dishRepository.GetByIdAsync(dishId);
                if (dish != null)
                    dishes.Add(dish);
            }

            Order.Dishes = dishes;
            // 🧾 Формуємо дані для LiqPay
            var liqpayParams = new Dictionary<string, object>
                {
                    { "public_key", _publicKey },
                    { "version", "3" },
                    { "action", "pay" },
                    { "amount", Order.TotalAmount },
                    { "currency", "UAH" },
                    { "description", $"Оплата замовлення {Order.Id}" },
                    { "order_id", Order.Id.ToString() },
                    { "result_url", $"{Request.Scheme}://{Request.Host}/PaymentSuccess?orderId={Order.Id}" },
                    { "server_url", $"{Request.Scheme}://{Request.Host}/liqpay-callback" },
                    { "sandbox", 1 } // <--- Це ключове
                };
            LiqPayData = ToBase64Json(liqpayParams);
            Console.WriteLine("liqpayParams: " + JsonConvert.SerializeObject(liqpayParams));
            Console.WriteLine("LiqPayData (Base64): " + LiqPayData);
            if (string.IsNullOrEmpty(LiqPayData))
                throw new Exception("LiqPayData is empty. Check serialization.");

            LiqPaySignature = GenerateSignature(_privateKey, LiqPayData);
            
            return Page();
        }

        // 📤 Метод POST: створюємо чек при оплаті готівкою
        public async Task<IActionResult> OnPostCreateReceiptAsync(Guid orderId, string paymentMethod)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null)
                return NotFound("Замовлення не знайдено");

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

        // 📦 Конвертація об'єкта в JSON, потім в Base64
        private string ToBase64Json(object obj)
        {
            var json = JsonConvert.SerializeObject(obj);
            var bytes = Encoding.UTF8.GetBytes(json);
            return Convert.ToBase64String(bytes);
        }

        // 🔐 Генерація підпису згідно з документацією LiqPay
        private string GenerateSignature(string privateKey, string data)
        {
            var bytes = Encoding.UTF8.GetBytes(privateKey + data + privateKey);
            using var sha1 = SHA1.Create();
            var hashBytes = sha1.ComputeHash(bytes);
            return Convert.ToBase64String(hashBytes);
        }
    }
}
