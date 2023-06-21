using asm.ViewModels;
using asm.Models;
using System.Threading.Tasks;

namespace asm.Services.PayPalSvc
{
    public interface IPayPalService
    {
        Task<string> CreatePaymentUrl(List<ViewCart> model, double total);
      
        PaymentResponse PaymentExecute(IQueryCollection collections);

        public string AddPaymentRespone(PaymentResponse paymentResponse);

    }
}
