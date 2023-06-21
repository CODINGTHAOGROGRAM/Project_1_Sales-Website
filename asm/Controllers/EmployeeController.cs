using asm.Constants;
using asm.Models;
using asm.ViewModels;
using asm.Services.EmployeeSvc;
using asm.Services.FoodSvc;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace asm.Controllers
{
    public class EmployeeController :BaseController
    {
        private readonly ILogger<EmployeeController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IFoodService _foodService;
        private readonly IEmployeeService _employeeService;
        public EmployeeController(IFoodService foodService, ILogger<EmployeeController> logger, IWebHostEnvironment webHostEnvironment, IEmployeeService employeeService)
        {
            _foodService = foodService;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _employeeService = employeeService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Profile()
        {
            return View();
        }

        
        public IActionResult Manage(int pg = 1)
        {
            var _listFood = _foodService.GetListFoodItems();
            const int pageSize = 10; // So data du lieu load len view
            if (pg < 1)
            {
                pg = 1;
            }
            int recsCount = _listFood.Count;
            var pagination = new Pagination(recsCount, pg, pageSize);
            int recSkip = (pg - 1 ) * pageSize;
            var data = _listFood.Skip(recSkip).Take(pagination.PageSize).ToList();
            this.ViewBag.Pagination = pagination;
            return View(data);
        }
    }
}
