using asm.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace asm.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        //public PartialViewResult BagCart()
        //{
        //    int item = 0;
        //    var cart = HttpContext.Session.GetString("cart");
        //    if (cart != null)
        //    {
        //        List<ViewCart> dataCart = JsonConvert.DeserializeObject<List<ViewCart>>(cart);
        //        for (int i = 0; i < dataCart.Count; i++)
        //        {
        //           item = 
        //        }
        //    }
        //}
    }
}
