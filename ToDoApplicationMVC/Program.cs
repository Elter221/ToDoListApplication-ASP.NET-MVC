.using Microsoft.EntityFrameworkCore;
using ToDoApplicationMVC.BLL.Services;
using ToDoApplicationMVC.BLL.Services.Interfaces;
using ToDoApplicationMVC.DAL;
using ToDoApplicationMVC.DAL.Entities;
using ToDoApplicationMVC.DAL.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContextPool<TodoListDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHostedService<DbInitService>();

builder.Services.AddScoped<IToDoListRepository, ToDoListRepository>();
builder.Services.AddScoped<IToDoRepository, ToDoRepository>();
builder.Services.AddScoped<ITagRepository, TagRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();

builder.Services.AddScoped<IToDoService, ToDoService>();
builder.Services.AddScoped<IToDoListService, ToDoListService>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

var app = builder.Build();

app.UseStaticFiles();

//документацию глянуть
app.UseStatusCodePages();

app.MapDefaultControllerRoute();

app.Run();
