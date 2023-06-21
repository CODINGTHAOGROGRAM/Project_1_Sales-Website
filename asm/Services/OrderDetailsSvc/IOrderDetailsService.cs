using asm.Models;

namespace asm.Services.OrderDetailsSvc
{
    public interface IOrderDetailsService
    {
        int AddOrderDetails(OrderDetails orderDetails);
    }
}
