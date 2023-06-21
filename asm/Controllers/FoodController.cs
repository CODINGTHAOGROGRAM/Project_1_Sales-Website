using asm.Constants;
using asm.Helpers;
using asm.Models;
using asm.Services.CartSvc;
using asm.Services.FoodSvc;
using asm.Services.OrderDetailsSvc;
using asm.Services.PayPalSvc;
using asm.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NuGet.Protocol;
using System;

namespace asm.Controllers
{
    public class FoodController : BaseController
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IUploadHelper _uploadHelper;
        private readonly IFoodService _foodService;
        private readonly IPayPalService  _payPalService;
        private readonly IOrderService _orderService;
        private readonly IOrderDetailsService _orderDetailsService;
        public FoodController(IOrderDetailsService orderDetailsService, IOrderService orderService, IFoodService foodService , IUploadHelper uploadHelper, IWebHostEnvironment webHostEnvironment, IPayPalService payPalService)
        {
            this._foodService = foodService;
            this._uploadHelper = uploadHelper;
            this._webHostEnvironment = webHostEnvironment;
            this._payPalService = payPalService;
            this._orderService = orderService;
            this._orderDetailsService = orderDetailsService;
        }
        #region Food
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Menu(int pg)
        {
            var _listFood = _foodService.GetListFoodItems();
            const int pageSize = 10;
            if (pg < 1)
            {
                pg = 1;
            }
            int recsCount = _listFood.Count;
            var pagination = new Pagination(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            var data = _listFood.Skip(recSkip).Take(pagination.PageSize).ToList();
            this.ViewBag.Pagination = pagination;
            return View(data);
        }








        public IActionResult Details(int id)
        {
            var foodItem = _foodService.GetFoodItemByID(id);
            if (foodItem == null)
            {
                return NotFound();
            }
            return PartialView("Detail", foodItem);
        }
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(FoodItem food)
        {
            if (food.ImageFile != null)
            {
                if (food.ImageFile.Length > 0)
                {
                    string rootPath = Path.Combine(_webHostEnvironment.WebRootPath, "img");
                    _uploadHelper.UploadImage(food.ImageFile, rootPath, "MenuFood");
                    food.Img = food.ImageFile.FileName;
                }
            }
            _foodService.CreateFood(food);
            return RedirectToAction("Manage", "Employee");
        }
        public IActionResult Edit(int id)
        {
            FoodItem _fooditem = _foodService.GetFoodItemByID(id);
            return View(_fooditem);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FoodItem _food)
        {
            FoodItem food = _foodService.GetFoodItemByID(_food.FoodId);
            if (ModelState.IsValid)
            {
                if (_food.ImageFile != null)
                {
                    if (_food.ImageFile.Length > 0)
                    {
                        string rootPath = Path.Combine(_webHostEnvironment.WebRootPath, "img");
                        _uploadHelper.UploadImage(food.ImageFile, rootPath, "MenuFood");
                        _food.Img = _food.ImageFile.FileName;
                    }
                }
            }
            _foodService.EditFoodItem(food);
            return RedirectToAction("Manage", "Employee");
        }


        #endregion

        #region cart
        public async Task<IActionResult> CreatePaymentUrl()
        {
            var cart = HttpContext.Session.GetString("cart");
            double total = TotalPrice();
            List<ViewCart> model = JsonConvert.DeserializeObject<List<ViewCart>>(cart);
            for (int i = 0; i < model.Count; i++)
            {
                if (model[i].FoodItem.Status != _foodService.GetFoodItemByID(model[i].FoodItem.FoodId).Status)
                {
                    var failMessage = $"{model[i].FoodItem.FoodName} is out of stock!";
                    return Json(new { message = failMessage });
                }
            }
            var url = await _payPalService.CreatePaymentUrl(model, total);

            return Content(url);
        }
        public IActionResult PaymentCallback()
        {
            var response = _payPalService.PaymentExecute(Request.Query);
            var payRes = JsonConvert.DeserializeObject<PaymentResponse>(response.ToJson());
            var cart = HttpContext.Session.GetString("cart");
            //string cusEmail = HttpContext.Session.GetString(SessionKey.Customer.CusFullName);
            //var cusContext = HttpContext.Session.GetString(SessionKey.Customer.CusContext);
            //var cusId = JsonConvert.DeserializeObject<Customer>(cusContext).CustomerID;
            string cusEmail = HttpContext.Session.GetString(SessionKey.Employee.FullName);
            var cusContext = HttpContext.Session.GetString(SessionKey.Employee.EmployeeContext);
            var cusId = JsonConvert.DeserializeObject<Employee>(cusContext).EmployeeID;
            List<ViewCart> dataCart = JsonConvert.DeserializeObject<List<ViewCart>>(cart);

            if (cusEmail == null || cusEmail == "")  // đã có session
            {
                return BadRequest();
            }
            if (payRes.Success)
            {


                double total = TotalPrice();


                var order = new Order()
                {
                    OrderId = payRes.OrderId,
                    Status = OrderStatus.Received,
                    CustomerId = cusId,
                    Method = payRes.PaymentMethod,
                    Total = total,
                    OrderDate = DateTime.Now,
                    Note = "",
                };

                _orderService.AddOrder(order);
                string orderId = order.OrderId;

                for (int i = 0; i < dataCart.Count; i++)
                {
                    OrderDetails details = new OrderDetails()
                    {
                        OrderId = orderId,
                        ProductId = dataCart[i].FoodItem.FoodId,
                        Quantity = dataCart[i].Quantity,
                        Total =(double)dataCart[i].FoodItem.Price * dataCart[i].Quantity,
                    };
                    //donhang.DonhangChitiets.Add(chitiet);
                    _orderDetailsService.AddOrderDetails(details);
                }
                _payPalService.AddPaymentRespone(payRes);
                HttpContext.Session.Remove("cart");

                TempData["SuccessMessage"] = "Successful payment!";

                return RedirectToAction("History", "Order");
            }
            else return BadRequest();
        }

        public IActionResult AddCart(int id)
        {
            int total = 0;
            var cart = HttpContext.Session.GetString("cart");
            if (cart == null)
            {
                var _foodItem = _foodService.GetFoodItemByID(id);
                List<ViewCart> listCart = new List<ViewCart>()
                {
                    new ViewCart
                    {
                        FoodItem = _foodItem,
                        Quantity = 1
                    }
                };
                HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(listCart));
            }
            else
            {
                List<ViewCart> dataCart = JsonConvert.DeserializeObject<List<ViewCart>>(cart);
                bool check = true;
                for (int i = 0; i < dataCart.Count; i++)
                {
                    if (dataCart[i].FoodItem.FoodId == id)
                    {
                        dataCart[i].Quantity = 1;
                        check = false;
                    }
                }
                if (check)
                {
                    var food = _foodService.GetFoodItemByID(id);
                    dataCart.Add(new ViewCart
                    {
                        FoodItem = food,
                        Quantity = 1
                    });
                    HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(dataCart));
                }
            }
            var cartCount = GetCartItemCount();
            return Json(new { cartCount });

        }
        public IActionResult Cart()
        {
            int tt = 0;
            List<ViewCart> dataCart = new List<ViewCart>();
            var cart = HttpContext.Session.GetString("cart");
            if (cart != null)
            {
                dataCart = JsonConvert.DeserializeObject<List<ViewCart>>(cart);
                tt = dataCart.Sum(x => x.Quantity);
            }
            ViewBag.Total = tt;
            return View(dataCart);
        }

        [NonAction]
        public int GetCartItemCount()
        {
            var cart = HttpContext.Session.GetString("cart");
            if (cart != null)
            {
                List<ViewCart> dataCart = JsonConvert.DeserializeObject<List<ViewCart>>(cart);
                return dataCart.Sum(s => s.Quantity);
            }
            else
            {
                return 0;
            }
        }

        public IActionResult BagCart()
        {
            int total = 0;
            var cart = HttpContext.Session.GetString("cart");
            if (cart != null)
            {
                List<ViewCart> dataCart = JsonConvert.DeserializeObject<List<ViewCart>>(cart);
                total = dataCart.Sum(s => s.Quantity);
            }
            ViewBag.TotalQuantity = total;
            return View(total);
        }
        [HttpPost]
        public IActionResult UpdateCart(int id, int quantity)
        {
            var cart = HttpContext.Session.GetString("cart");
            double total = 0;
            if (cart != null)
            {
                List<ViewCart> dataCart = JsonConvert.DeserializeObject<List<ViewCart>>(cart);
                for (int i = 0; i < dataCart.Count; i++)
                {
                    if (dataCart[i].FoodItem.FoodId == id)
                    {
                        dataCart[i].Quantity = quantity;
                        break;
                    }
                }
                HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(dataCart));

                total = TotalPrice();
                return Ok(total);

            }
            return BadRequest();
        }
        [HttpPost]
        public IActionResult DeleteCart(int id)
        {
            double total = 0;
            var cart = HttpContext.Session.GetString("cart");
            if (cart != null)
            {
                List<ViewCart> dataCart = JsonConvert.DeserializeObject<List<ViewCart>>(cart);
                var item = dataCart.FirstOrDefault(c => c.FoodItem.FoodId == id);
                if (item.Quantity > 1)
                {
                    item.Quantity--;
                }
                else
                {
                    for (int i = 0; i < dataCart.Count; i++)
                    {

                        if (dataCart[i].FoodItem.FoodId == id)
                        {
                            dataCart.RemoveAt(i);
                        }
                    }
                }

                HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(dataCart));
                total = TotalPrice();
                return Ok(total);
            }
            return RedirectToAction("BagCart");
        }
        [NonAction]
        private double TotalPrice()
        {
            double total = 0;
            var cart = HttpContext.Session.GetString("cart");
            if (cart != null)
            {
                List<ViewCart> dataCart = JsonConvert.DeserializeObject<List<ViewCart>>(cart);
                for (int i = 0; i < dataCart.Count; i++)
                {
                    total += (double)(dataCart[i].FoodItem.Price * dataCart[i].Quantity);
                }
            }
            return total;
        }


        //public IActionResult OrderCart()
        //{
        //    string emp_Email = HttpContext.Session.GetString(SessionKey.Employee.Email);
        //    if (emp_Email == null || emp_Email == "")  // đã có session
        //    {
        //        return BadRequest();
        //    }
        //    var cart = HttpContext.Session.GetString("cart");
        //    if (cart != null && cart.Count() > 0)
        //    {
        //        #region DonHang
        //        var EmployeeContext = HttpContext.Session.GetString(SessionKey.Employee.EmployeeContext);
        //        var empId = JsonConvert.DeserializeObject<Employee>(EmployeeContext).EmployeeID;

        //        double total = TotalPrice();

        //        var orderItem = new Order()
        //        {
        //            Status = OrderStatus.Received,
        //            CustomerId = empId,
        //            Total = total,
        //            OrderDate = DateTime.Now,
        //            Note = ""
        //        };

        //        _.AddDonhang(donhang);
        //        int donhangId = donhang.DonhangID;

        //        #region Chitiet
        //        List<ViewCart> dataCart = JsonConvert.DeserializeObject<List<ViewCart>>(cart);
        //        for (int i = 0; i < dataCart.Count; i++)
        //        {
        //            DonhangChitiet chitiet = new DonhangChitiet()
        //            {
        //                DonhangID = donhangId,
        //                MonAnID = dataCart[i].MonAn.MonAnID,
        //                Soluong = dataCart[i].Quantity,
        //                Thanhtien = dataCart[i].MonAn.Gia * dataCart[i].Quantity,
        //                Ghichu = "",
        //            };
        //            //donhang.DonhangChitiets.Add(chitiet);
        //            _donhangChitietSvc.AddDonhangChitietSvc(chitiet);
        //        }

        //        #endregion
        //        #endregion

        //        HttpContext.Session.Remove("cart");

        //        return Ok();
        //    }
        //    return BadRequest();
        //}


        #endregion


    }

}
