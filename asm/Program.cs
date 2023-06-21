using asm.Helpers;
using asm.Models;
using asm.Services.CartSvc;
using asm.Services.EmployeeSvc;
using asm.Services.FoodSvc;
using asm.Services.OrderDetailsSvc;
using asm.Services.PayPalSvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
string Connect = builder.Configuration.GetConnectionString("DefautConnectionString");
builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(Connect));
builder.Services.AddScoped<IFoodService, FoodService>();
builder.Services.AddTransient<IUploadHelper, UploadHelper>();
builder.Services.AddScoped<IDataEncode, DataEncode>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IPayPalService, PayPalService>();
builder.Services.AddScoped<IOrderDetailsService, OrderDetailsService>();

builder.Services.AddDistributedMemoryCache(); // Dang ky dich vu luu code trong cache
builder.Services.AddSession(option =>
{
    option.IdleTimeout = TimeSpan.FromMinutes(30);
});
var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    app.UseStaticFiles();
}
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
