using menu.Interface;
using menu.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

[Route("api/receipts")]
[ApiController]
public class ReceiptController : ControllerBase
{
    private readonly IReceiptRepository _receiptRepository;
    private readonly IOrderRepository _orderRepository;

    public ReceiptController(
        IReceiptRepository receiptRepository,
        IOrderRepository orderRepository)
    {
        _receiptRepository = receiptRepository;
        _orderRepository = orderRepository;
    }

    // Отримати всі чеки
    [HttpGet]
    public async Task<ActionResult> GetAllReceipts()
    {
        var receipts = await _receiptRepository.GetAllAsync();
        return Ok(receipts);
    }

    // Отримати чек за ID
    [HttpGet("{id}")]
    public async Task<ActionResult> GetReceiptById(Guid id)
    {
        var receipt = await _receiptRepository.GetByIdAsync(id);
        if (receipt == null)
            return NotFound("Чек не знайдено");

        return Ok(receipt);
    }

    // Створити новий чек (TotalAmount заповнюється з Order)
    [HttpPost]
    public async Task<ActionResult> CreateReceipt([FromForm] Receipt receipt)
    {
        if (receipt == null || receipt.OrderId == Guid.Empty)
            return BadRequest("Некоректні дані");

        var order = await _orderRepository.GetByIdAsync(receipt.OrderId);
        if (order == null)
            return NotFound("Замовлення не знайдено");

        receipt.TotalAmount = order.TotalAmount; // Автоматично встановлюємо суму замовлення
        receipt.Date = DateTime.UtcNow;

        await _receiptRepository.AddAsync(receipt);

        return CreatedAtAction(nameof(GetReceiptById), new { id = receipt.Id }, receipt);
    }

    // Оновити чек
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateReceipt(Guid id, [FromForm] Receipt receipt)
    {
        if (receipt == null || receipt.OrderId == Guid.Empty)
            return BadRequest("Некоректні дані");

        var existingReceipt = await _receiptRepository.GetByIdAsync(id);
        if (existingReceipt == null)
            return NotFound("Чек не знайдено");

        var order = await _orderRepository.GetByIdAsync(receipt.OrderId);
        if (order == null)
            return NotFound("Замовлення не знайдено");

        existingReceipt.OrderId = receipt.OrderId;
        existingReceipt.TotalAmount = order.TotalAmount; // Оновлюємо суму
        existingReceipt.Date = DateTime.UtcNow;

        await _receiptRepository.UpdateAsync(existingReceipt);

        return NoContent();
    }

    // Видалити чек
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteReceipt(Guid id)
    {
        var existingReceipt = await _receiptRepository.GetByIdAsync(id);
        if (existingReceipt == null)
            return NotFound("Чек не знайдено");

        await _receiptRepository.DeleteAsync(id);
        return NoContent();
    }
}
