using ToDoApplicationMVC.DAL.Entities;
using ToDoApplicationMVC.DAL.Interfaces;

namespace ToDoApplicationMVC.DAL;

public class UserRepository(TodoListDbContext context) : Repository<User>(context), IUserRepository
{
}
