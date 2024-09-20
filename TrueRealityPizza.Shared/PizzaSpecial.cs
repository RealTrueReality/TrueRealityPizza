using System.ComponentModel.DataAnnotations;

namespace TrueRealityPizza.Shared
{
    /// <summary>
    /// Represents a pre-configured template for a pizza a user can order
    /// </summary>
    public class PizzaSpecial
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "披萨名称是必填项。")]
        [StringLength(15, ErrorMessage = "披萨名称不能超过100个字符。")]
        public string Name { get; set; } = string.Empty;

        [Range(0.01, 10000.00, ErrorMessage = "基础价格必须在0.01到10000之间。")]
        public decimal BasePrice { get; set; }

        [Required(ErrorMessage = "描述是必填项。")]
        [StringLength(500, ErrorMessage = "描述不能超过30个字符。")]
        public string Description { get; set; } = string.Empty;
        
        public string ImageUrl { get; set; } = "img/pizzas/cheese.jpg";

        public string GetFormattedBasePrice() => BasePrice.ToString("0.00");
    }
}