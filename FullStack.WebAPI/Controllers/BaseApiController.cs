using System;
using System.Net.Http;
using System.Web.Http;
using FullStack.WebAPI.Infrastructure;
using FullStack.WebAPI.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;

namespace FullStack.WebAPI.Controllers
{
    public class BaseApiController : ApiController
    {
        private ModelFactory _modelFactory;
        private ApplicationUserManager _AppUserManager = null;
        private ApplicationRoleManager _AppRoleManager = null;
        private IAuthenticationManager _Authentication = null;

        protected ModelFactory TheModelFactory
        {
            get
            {
                if (_modelFactory == null)
                    _modelFactory = new ModelFactory(this.Request, this.AppUserManager);
                return _modelFactory;
            }
        }

        protected ApplicationUserManager AppUserManager
        {
            get
            {
                if (_AppUserManager == null)
                    _AppUserManager = Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
                return _AppUserManager;
            }
        }

        protected ApplicationRoleManager AppRoleManager
        {
            get
            {
                if (_AppRoleManager == null)
                    _AppRoleManager = Request.GetOwinContext().GetUserManager<ApplicationRoleManager>();
                return _AppRoleManager;
            }
        }
        
        protected IOwinContext OwinContext
        {
            get
            {
                return Request.GetOwinContext();
            }
        }

        protected IAuthenticationManager Authentication
        {
            get
            {
                if (_Authentication == null)
                    _Authentication = Request.GetOwinContext().Authentication;
                return _Authentication;
            }
        }

        public BaseApiController()
        {
        }

        protected IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
                return InternalServerError();

            if (result.Succeeded)
                return null;
        
            if (result.Errors != null)
            {
                foreach (string error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error);
                }
            }

            if (ModelState.IsValid)
            {
                // No ModelState errors are available to send, so just return an empty BadRequest.
                return BadRequest();
            }

            return BadRequest(ModelState);
        }
    }
}
