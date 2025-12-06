using Microsoft.AspNetCore.Identity;
using ToDoApplicationMVC.DAL.Entities;

namespace ToDoApplicationMVC.Extensions;

public static class SeedData
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetRequiredService<TodoListDbContext>();

        await context.Database.EnsureCreatedAsync();

        await AddRoles(serviceProvider);

        await AddDefaultAdmin(serviceProvider);

        await AddDefaultOwner(serviceProvider);
    }

    private static async Task AddRoles(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();

        string[] roles = { "Admin", "User", "Owner", "Editor" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole<int>(role));
            }
        }
    }

    private static async Task AddDefaultAdmin(IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

        var adminEmail = "admin@admin.com";
        var adminPassword = "123";

        var adminUser = await userManager.FindByEmailAsync(adminEmail);

        if (adminUser == null)
        {
            adminUser = new User
            {
                UserName = adminEmail,
                Email = adminEmail,
            };

            var result = await userManager.CreateAsync(adminUser, adminPassword);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }

    private static async Task AddDefaultOwner(IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

        var ownerEmail = "elter@gmail.com";
        var ownerPassword = "12345";

        var adminUser = await userManager.FindByEmailAsync(ownerEmail);

        if (adminUser == null)
        {
            adminUser = new User
            {
                UserName = ownerEmail,
                Email = ownerEmail,
            };

            var result = await userManager.CreateAsync(adminUser, ownerPassword);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Owner");
            }
        }
    }
}
