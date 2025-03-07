using menu.Interface;
using menu.Model;
using menu.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// ��������� ��������� Razor Pages
builder.Services.AddRazorPages();

// ���������� ���� ������ PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IDishRepository, DishRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IReceiptRepository, ReceiptRepository>();

// ��������� ��������� ������
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // ����� �������� ������
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// ��������� ����������� � ���������� API
builder.Services.AddControllers();
builder.Services.AddControllersWithViews();
// ��������� Swagger
builder.Services.AddEndpointsApiExplorer();


var app = builder.Build();

// ��������� middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// �������� ��������� ������
app.UseSession();

app.UseAuthorization();

app.MapRazorPages();
app.MapControllers(); // ��������� ��������� API-������������


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
