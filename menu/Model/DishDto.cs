using System.ComponentModel.DataAnnotations;

public class DishDto
{
    [Display(Name = "Назва")]
    public string Title { get; set; }

    [Display(Name = "Опис")]
    public string Description { get; set; }

    [Display(Name = "Ціна")]
    public decimal Price { get; set; }

    [Display(Name = "В наявності")]
    public bool IsAvailable { get; set; }

    [Display(Name = "Вага (г)")]
    public double Weight { get; set; }
}
