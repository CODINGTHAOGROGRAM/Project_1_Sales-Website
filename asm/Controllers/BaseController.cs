using asm.Constants;
using Microsoft.AspNetCore.Mvc;

namespace asm.Controllers
{
    public class BaseController : Controller
    {
        public BaseController() { }
        protected string GetUserName()
        {
            return HttpContext.Session.GetString(SessionKey.Employee.UserName);
        }
        protected string GetFullName()
        {
            return HttpContext.Session.GetString(SessionKey.Employee.FullName);
        }
        protected string GetCusEmail()
        {
            return HttpContext.Session.GetString(SessionKey.Customer.CusEmail);
        }
    }
}
