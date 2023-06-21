using asm.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace asm.Fillters
{
    public class AuthenticationFilterAttribute:ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // The action filter logic.
            Controller controller = filterContext.Controller as Controller;
            var session = filterContext.HttpContext.Session;
            string userName = filterContext.HttpContext.Session.GetString(SessionKey.Employee.UserName);
            var sessionStatus = ((userName != null && userName != "") ? true : false);
            if (controller != null)
            {
                if (session == null || !sessionStatus)
                {
                    filterContext.Result =
                           new RedirectToRouteResult(
                               new RouteValueDictionary{
                                   { "controller", "Admin" },
                                   { "action", "Login" }}
                               );

                }
            }
            base.OnActionExecuting(filterContext);
        }
    }


    public class AuthenticationFilterAttribute_Cus : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // The action filter logic.
            Controller controller = filterContext.Controller as Controller;
            var session = filterContext.HttpContext.Session;
            string Email = filterContext.HttpContext.Session.GetString(SessionKey.Customer.CusEmail);
            var sessionStatus = ((Email != null && Email != "") ? true : false);
            if (controller != null)
            {
                if (session == null || !sessionStatus)
                {
                    filterContext.Result =
                           new RedirectToRouteResult(
                               new RouteValueDictionary{
                                   { "controller", "Home" },
                                   { "action", "Login" }}
                               );

                }
            }
            base.OnActionExecuting(filterContext);
        }



    }
}
