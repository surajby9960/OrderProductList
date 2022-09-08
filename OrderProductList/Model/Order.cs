namespace OrderProductList.Model
{
    public class Order 

    {
        public int orderId { get; set; }
        public string? custName { get; set; }
        public string? shippingAddress { get; set; }
        public DateTime orderDate { get; set; }
        public double finalAmount { get; set; }
        public List<Product> products { get; set; }
    }
    
}
