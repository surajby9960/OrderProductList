using OrderProductList.Model;

namespace OrderProductList.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        public Task<IEnumerable<Order>> GetAllOrder();
        public Task<Order> GetOrderById(int id);
        public Task<int> InsertOrder(Order order);
        public Task<int> UpdateOrder(Order order);
        public Task DeleteOrder(int id);
    }
}
