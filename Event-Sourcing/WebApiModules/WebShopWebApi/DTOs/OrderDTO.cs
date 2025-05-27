using OrderModule.Models;

namespace WebShopWebApi.DTOs;

public class OrderDTO
{
    public Guid Id { get; set; }
    public string Status { get; set; }
    public List<OrderItem> Items { get; set; }
}
