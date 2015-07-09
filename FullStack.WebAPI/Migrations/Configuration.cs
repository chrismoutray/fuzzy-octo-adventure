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

            var user = new ApplicationUser()
            {
                UserName = "admin",
                Email = "admin@fullstack.webapi",
                EmailConfirmed = true,
                FirstName = "Super",
                LastName = "Admin",
                Level = 1,
                JoinDate = DateTime.Now
            };

            manager.Create(user, "passs");
        }
    }
}
