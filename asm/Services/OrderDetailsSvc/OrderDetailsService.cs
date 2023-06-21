using asm.Models;

namespace asm.Services.OrderDetailsSvc
{
    public class OrderDetailsService : IOrderDetailsService
    {
        private readonly DataContext _dataContext;
        public OrderDetailsService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public int AddOrderDetails(OrderDetails orderDetails)
        {
            int ret = 0;
            try
            {
                _dataContext.Add(orderDetails);
                _dataContext.SaveChanges();
                ret = orderDetails.OrderDetailId;
            }
            catch (Exception ex)
            {
                ret = 0;
            }
            return ret;
        }
    }
}
