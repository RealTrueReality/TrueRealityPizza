using System.ComponentModel.DataAnnotations;

namespace TrueRealityPizza.Shared;

public class Address
{
    public int Id { get; set; }
		
    [Required(ErrorMessage = "姓名是必填项"), MaxLength(10, ErrorMessage = "姓名不能超过10个字符")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "地址是必填项"), MaxLength(100, ErrorMessage = "地址不能超过100个字符")]
    public string Line1 { get; set; } = string.Empty;

    [MaxLength(100, ErrorMessage = "地址行2不能超过100个字符")]
    public string Line2 { get; set; } = string.Empty;

    [Required(ErrorMessage = "城市是必填项"), MaxLength(50, ErrorMessage = "城市名称不能超过50个字符")]
    public string City { get; set; } = string.Empty;

    [Required(ErrorMessage = "省/区域是必填项"), MaxLength(20, ErrorMessage = "省/区域名称不能超过20个字符")]
    public string Region { get; set; } = string.Empty;

    [Required(ErrorMessage = "邮政编码是必填项"), MaxLength(20, ErrorMessage = "邮政编码不能超过20个字符")]
    public string PostalCode { get; set; } = string.Empty;
}