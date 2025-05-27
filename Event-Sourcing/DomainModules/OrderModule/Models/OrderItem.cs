namespace OrderModule.Models
{
    public class OrderItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public float Price { get; set; }
        public int Amount { get; set; }
    }
}
