namespace FullStack.WebAPI.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using FullStack.WebAPI.Infrastructure;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
 
            var user = new ApplicationUser()
            {
                UserName = "SuperAdmin",
                Email = "SuperAdmin@FullStack.WebAPI",
                EmailConfirmed = true,
                FirstName = "Super",
                LastName = "Admin",
                Level = 1,
                JoinDate = DateTime.Now
            };

            manager.Create(user, "Password99!");

            if (roleManager.Roles.Count() == 0)
            {
                roleManager.Create(new IdentityRole { Name = "SuperAdmin" });
                roleManager.Create(new IdentityRole { Name = "Admin" });
                roleManager.Create(new IdentityRole { Name = "User" });
            }

            var adminUser = manager.FindByName("SuperAdmin");

            manager.AddToRoles(adminUser.Id, new string[] { "SuperAdmin", "Admin" });
        }
    }
}
