namespace OrderProductList.Model
{
    public class Product
    {
        public int productId { get; set; }
        public string? productName { get; set; }
        public double productPrice { get; set; }
        public int quantity { get; set; }
        public double totalAmount { get; set; }
        public int orderId { get; set; }
    }

}
