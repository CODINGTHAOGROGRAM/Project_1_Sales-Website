using asm.Models;
using Microsoft.EntityFrameworkCore;

namespace asm.Services.CartSvc
{
    public class OrderService : IOrderService
    {
        private readonly DataContext _dataContext;
        public OrderService(DataContext dataContext) {  _dataContext = dataContext; }
        public string AddOrder(Order order)
        {
           string status = "";
            try
            {

                _dataContext.Add(order);
                _dataContext.SaveChanges();
                status = order.OrderId;
            }
            catch
            {
                status = "";
            }
            return status;
        }

        public Order GetOrderById(int orderID)
        {
            try
            {
                return _dataContext.Orders.Where(o => o.OrderId == orderID.ToString())
                    .Include(o => o.Customer)
                    .Include(o => o.OrderDetails)
                    .ThenInclude(o => o.foodItem)
                    .FirstOrDefault();
            }
            catch (System.Exception ex)
            {
                return new Order();
            }
        }

        public List<Order> GetOrdersAll()
        {
            var list = _dataContext.Orders.OrderByDescending(x => x.OrderDate)
            .Include(x => x.Customer)
            .Include(x => x.OrderDetails)
            .ToList();
            return list;
        }

        public List<Order> GetOrdersByCustomerID(int customerID)
        {
            throw new NotImplementedException();
        }

        public string UpdateOrder(Order order, int id)
        {
            string ret = "";
            try
            {
                _dataContext.Update(order);
                _dataContext.SaveChanges();
                    ret = order.OrderId;
            }catch(Exception ex)
            {
                ret = "";
            }
            return ret;
        }
    }
}
