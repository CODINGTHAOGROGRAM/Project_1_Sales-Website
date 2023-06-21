using asm.ViewModels;
using asm.Models;
using Microsoft.EntityFrameworkCore;
using System.Net;
using PayPal.Core;
using PayPal.v1.Payments;

namespace asm.Services.PayPalSvc
{
    public class PayPalService : IPayPalService
    {
        private readonly IConfiguration _configuration;
        private readonly DataContext _dataContext;
        private const double ExchangeRate = 22_863.0;
        public PayPalService(IConfiguration configuration, DataContext dataContext)
        {
            _configuration = configuration;
            _dataContext = dataContext;
        }
        public string AddPaymentRespone(PaymentResponse paymentResponse)
        {
            string status = "";
            try
            {

                _dataContext.Add(paymentResponse);
                _dataContext.SaveChanges();
                status = paymentResponse.PaymentId;
            }
            catch
            {
                status = null;
            }
            return status;
        }
        public static double ConvertVndToDollar(double vnd)
        {
            var total = Math.Round(vnd / ExchangeRate, 2);

            return total;
        }

        public async Task<string> CreatePaymentUrl(List<ViewCart> model, double total)
        {
            var envSandbox = new SandboxEnvironment(_configuration["Paypal:ClientId"], _configuration["Paypal:SecretKey"]);
            var client = new PayPalHttpClient(envSandbox);
            var paypalOrderId = DateTime.Now.Ticks;
            var urlCallBack = _configuration["PaymentCallBack:ReturnUrl"];

            var itemList = new ItemList();
            itemList.Items = new List<Item>();

            foreach (var cartItem in model)
            {
                var item = new Item()
                {
                    Name = " | Name: " + cartItem.FoodItem.FoodName,
                    Currency = "USD",
                    Price = ConvertVndToDollar((double)cartItem.FoodItem.Price).ToString(),
                    Quantity = cartItem.Quantity.ToString(),
                };

                itemList.Items.Add(item);
            }

            var payment = new PayPal.v1.Payments.Payment()
            {
                Intent = "sale",
                Transactions = new List<Transaction>()
                {
                    new Transaction()
                    {
                        Amount = new Amount()
                        {
                            Total = ConvertVndToDollar(total).ToString(),
                            Currency = "USD",
                            Details = new AmountDetails
                            {
                                Tax = "0",
                                Shipping = "0",
                                Subtotal = ConvertVndToDollar(total).ToString(),
                            }
                        },
                        ItemList =itemList,
                        InvoiceNumber = paypalOrderId.ToString()
                    }
                },
                RedirectUrls = new RedirectUrls()
                {
                    ReturnUrl =
                        $"{urlCallBack}?payment_method=PayPal&success=1&order_id={paypalOrderId}",
                    CancelUrl =
                        $"{urlCallBack}?payment_method=PayPal&success=0&order_id={paypalOrderId}"
                },
                Payer = new Payer()
                {
                    PaymentMethod = "paypal"
                }
            };
            var request = new PaymentCreateRequest();
            request.RequestBody(payment);

            var paymentUrl = "";
            var response = await client.Execute(request);
            var statusCode = response.StatusCode;

            if (statusCode is not (HttpStatusCode.Accepted or HttpStatusCode.OK or HttpStatusCode.Created))
                return paymentUrl;

            var result = response.Result<Payment>();
            using var links = result.Links.GetEnumerator();

            while (links.MoveNext())
            {
                var lnk = links.Current;
                if (lnk == null) continue;
                if (!lnk.Rel.ToLower().Trim().Equals("approval_url")) continue;
                paymentUrl = lnk.Href;
            }

            return paymentUrl;
        }

        public PaymentResponse PaymentExecute(IQueryCollection collections)
        {
            var response = new PaymentResponse();

            foreach (var (key, value) in collections)
            {

                if (!string.IsNullOrEmpty(key) && key.ToLower().Equals("order_id"))
                {
                    response.OrderId = value;
                }

                if (!string.IsNullOrEmpty(key) && key.ToLower().Equals("payment_method"))
                {
                    response.PaymentMethod = value;
                }

                if (!string.IsNullOrEmpty(key) && key.ToLower().Equals("success"))
                {
                    response.Success = Convert.ToInt32(value) > 0;
                }

                if (!string.IsNullOrEmpty(key) && key.ToLower().Equals("paymentid"))
                {
                    response.PaymentId = value;
                }

                if (!string.IsNullOrEmpty(key) && key.ToLower().Equals("payerid"))
                {
                    response.PayerId = value;
                }
            }

            return response;
        }
    }
}
