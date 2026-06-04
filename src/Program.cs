using DatPhongKhachSanVungTau.Models;
using DatPhongKhachSanVungTau.Data;
using DatPhongKhachSanVungTau.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Kết nối SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Session — dùng cho đăng nhập (Bước 4)
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.Name = ".DatPhongVungTau.Session";
});

builder.Services.AddScoped<DatPhongService>();
builder.Services.AddScoped<AnhUploadService>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// SeedData (Bước 3)
DbInitializer.KhoiTaoDuLieu(app);

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();
app.UseAuthorization();

// Route quản trị Admin (chuẩn bị Bước 9)
app.MapControllerRoute(
    name: "admin",
    pattern: "Admin/{controller=Dashboard}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
