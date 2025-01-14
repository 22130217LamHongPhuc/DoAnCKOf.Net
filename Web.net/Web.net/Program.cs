using ECommerceMVC.Services;
using Microsoft.Extensions.Options;
using Web.net.Models;
using Web.net.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddHttpClient<ProductService>();
builder.Services.AddHttpClient<OrderService>();
builder.Services.AddHttpClient<VoucherService>();
builder.Services.AddHttpClient<SearchSevice>();

builder.Services.AddHttpClient<HomeService>();
builder.Services.AddHttpClient<UserService>();



builder.Services.AddHttpClient<ProductReviewService>();
builder.Services.AddScoped<IVnPayService, VnPayService>();



// Admin
builder.Services.AddHttpClient<AdminService>();

builder.Services.AddDistributedMemoryCache();  //  sử dụng bộ nhớ cho session

builder.Services.AddSession(options =>
        options.IdleTimeout = TimeSpan.FromDays(2)  // Thời gian hết hạn của session

);
builder.Services.AddControllersWithViews();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}





app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseSession();  // Kích hoạt session

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

app.MapControllerRoute(
    name: "order-history",
    pattern: "Order/History",
    defaults: new { controller = "InforOrder", action = "History" });
