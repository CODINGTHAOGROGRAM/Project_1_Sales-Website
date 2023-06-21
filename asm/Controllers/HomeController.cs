using asm.Constants;
using asm.Models;
using asm.Services.FoodSvc;
using asm.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace asm.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
       
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            
        }

        public IActionResult Index()
        {
            return View();
        }

        //public IActionResult Login()
        //{
        //    string userName = HttpContext.Session.GetString(SessionKey.Employee.UserName);
        //    if (userName != null && userName != "")
        //    {
        //        return RedirectToAction("Index", "Home");
        //    }
        //    return View();
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Login(ViewLogin viewLogin)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        Employee employee = _employeeService.Login(viewLogin);
        //        if (employee != null)
        //        {
        //            HttpContext.Session.SetString(SessionKey.Employee.UserName, employee.UserName);
        //            HttpContext.Session.SetString(SessionKey.Employee.FullName, employee.FullName);
        //            HttpContext.Session.SetString(SessionKey.Employee.Img, employee.Image);
        //            //HttpContext.Session.SetString(SessionKey.Employee.Role, employee.Position.ToString());
        //            if (employee.Position.ToString() == "Manager")
        //            {
        //                HttpContext.Session.SetString(SessionKey.Employee.Role, "Admin");
        //            }
        //            else
        //            {
        //                HttpContext.Session.SetString(SessionKey.Employee.Role, "Employee");
        //            }
        //            HttpContext.Session.SetString(SessionKey.Employee.EmployeeContext,
        //                JsonConvert.SerializeObject(employee));

        //            if (employee.Position == Position.Manager)
        //            {
        //                return RedirectToAction("Profile", "Employee");

        //            }
        //            else
        //            {
        //                return RedirectToAction("Index", "Home");
        //            }

        //        }
        //    }
        //    return View(viewLogin);
        //}


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}