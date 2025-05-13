using Microsoft.EntityFrameworkCore;
using ToDoApplicationMVC.DataAccess;
using ToDoApplicationMVC.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContextPool<TodoListDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHostedService<DbInitService>();

var app = builder.Build();

app.UseStaticFiles();

app.MapDefaultControllerRoute();

app.Run();
