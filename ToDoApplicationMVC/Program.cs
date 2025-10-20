using Microsoft.EntityFrameworkCore;
using ToDoApplicationMVC.BLL.Services;
using ToDoApplicationMVC.BLL.Services.Interfaces;
using ToDoApplicationMVC.DataAccess.Entities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContextPool<TodoListDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHostedService<DbInitService>();

builder.Services.AddScoped<IToDoService, ToDoService>();

builder.Services.AddScoped<IToDoListService, ToDoListService>();

var app = builder.Build();

app.UseStaticFiles();

//документацию глянуть
app.UseStatusCodePages();

app.MapDefaultControllerRoute();

app.Run();
