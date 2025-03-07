using menu.Interface;
using menu.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;

namespace menu.Pages
{
    public class PaymentSuccessModel : PageModel
    {
        private readonly IReceiptRepository _receiptRepository;

        public PaymentSuccessModel(IReceiptRepository receiptRepository)
        {
            _receiptRepository = receiptRepository;
        }

        public Receipt Receipt { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid receiptId)
        {
            Receipt = await _receiptRepository.GetByIdAsync(receiptId);
            if (Receipt == null)
            {
                return NotFound("Чек не знайдено");
            }

            return Page();
        }
    }
}
