using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using FullStack.WebAPI.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace FullStack.WebAPI.Infrastructure
{
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var appDbContext = context.Get<ApplicationDbContext>();
            var appUserManager = new ApplicationUserManager(new UserStore<ApplicationUser>(appDbContext));

            appUserManager.EmailService = new EmailService();

            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                int hours = int.Parse(ConfigurationManager.AppSettings["applicationUserManager:emailLifespanInHours"]);
                appUserManager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"))
                {
                    //Code for email confirmation and reset password life time
                    TokenLifespan = TimeSpan.FromHours(hours)
                };
            }

            //Configure validation logic for usernames
            //appUserManager.UserValidator = new FullStack.WebAPI.Validators.MyCustomUserValidator(appUserManager)
            appUserManager.UserValidator = new UserValidator<ApplicationUser>(appUserManager)
            {
                AllowOnlyAlphanumericUserNames = true,
                RequireUniqueEmail = true
            };

            //Configure validation logic for passwords
            //appUserManager.PasswordValidator = new FullStack.WebAPI.Validators.MyCustomPasswordValidator()
            appUserManager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = false,
                RequireLowercase = true,
                RequireUppercase = true
            };

#if DEBUG
            appUserManager.PasswordValidator = new PasswordValidator { RequiredLength = 1 };
#endif

            return appUserManager;
        }
    }
}