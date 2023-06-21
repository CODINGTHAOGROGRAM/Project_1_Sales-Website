using asm.Models;

namespace asm.Services.CartSvc
{
    public interface IOrderService
    {
        public List<Order> GetOrdersAll();
        public List<Order> GetOrdersByCustomerID(int customerID);
        public Order GetOrderById(int orderID);
        public string AddOrder(Order order);
        public string UpdateOrder(Order order, int id);

    }
}
