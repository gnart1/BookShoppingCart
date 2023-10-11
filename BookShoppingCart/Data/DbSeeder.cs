using BookShoppingCart.Constants;
using Microsoft.AspNetCore.Identity;

namespace BookShoppingCart.Data
{
    public class DbSeeder
    {
        public static async Task SeedDefaultData(IServiceProvider serviceProvider)
        {
            var userMgr = serviceProvider.GetService<UserManager<IdentityUser>>();
            var roleMgr = serviceProvider.GetService<RoleManager<IdentityRole>>();
            //Them roles vao db
            await roleMgr.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            await roleMgr.CreateAsync(new IdentityRole(Roles.User.ToString()));

            //tao admin user
            var admin = new IdentityUser
            {
                UserName = "admin@gmail.com",
                Email = "admin@gmail.com",
                EmailConfirmed = true,
            };
            var userInDb = await userMgr.FindByEmailAsync(admin.Email);
            if (userInDb == null)
            {
                await userMgr.CreateAsync(admin, "Admin@123");
                await userMgr.AddToRoleAsync(admin,Roles.Admin.ToString());
            }

        }
    }
}
