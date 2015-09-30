using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using FullStack.WebAPI.Infrastructure;
using FullStack.WebAPI.Models;
using Microsoft.AspNet.Identity;

namespace FullStack.WebAPI.Controllers
{
    [RoutePrefix("api/accounts")]
    public class AccountsController : BaseApiController
    {
        [AllowAnonymous]
        [Route("signup")]
        [HttpPost]
        public async Task<IHttpActionResult> SignUp(SignUpUserBindingModel signupUserModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new ApplicationUser()
            {
                UserName = signupUserModel.Email,
                Email = signupUserModel.Email,
                FirstName = signupUserModel.FirstName,
                LastName = signupUserModel.LastName,
                DisplayName = signupUserModel.DisplayName,
                JoinDate = DateTime.Now.Date,
            };

            IdentityResult createUserResult = await this.AppUserManager.CreateAsync(user, signupUserModel.Password);

            if (!createUserResult.Succeeded)
            {
                return GetErrorResult(createUserResult);
            }

            string code = await this.AppUserManager.GenerateEmailConfirmationTokenAsync(user.Id);

            Uri confirmSignUpRoute = new Uri(Url.Link("ConfirmSignUpRoute", new { userId = user.Id, code = code }));

            //string confirmSignUpWebAppUrl = string.Format("http://www.fullstack.com/confirm-signup{0}", confirmSignUpRoute.Query);
            string confirmSignUpWebAppUrl = string.Format("http://api.fullstack.co.uk/api/accounts/confirm-signup{0}", confirmSignUpRoute.Query);

            //string message = string.Format("{ type: 'AccountSignUp', data: { confirmSignUpWebAppUrl: '{0}' } }", confirmSignUpWebAppUrl); 
            await this.AppUserManager.SendEmailAsync(user.Id, "Account SignUp", "Please confirm your account by clicking <a href=\"" + confirmSignUpWebAppUrl + "\">here</a>");

            Uri locationHeader = new Uri(Url.Link("GetUserById", new { id = user.Id }));
            
            return Created(locationHeader, TheModelFactory.Create(user));
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("confirm-signup", Name = "ConfirmSignUpRoute")]
        public async Task<IHttpActionResult> ConfirmSignUp(string userId = "", string code = "")
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(code))
            {
                ModelState.AddModelError("", "User Id and Code are required");
                return BadRequest(ModelState);
            }

            IdentityResult confirmEmailResult = await this.AppUserManager.ConfirmEmailAsync(userId, code);

            if (!confirmEmailResult.Succeeded)
            {
                return GetErrorResult(confirmEmailResult);
            }

            IdentityResult addToRolesResult = await this.AppUserManager.AddToRolesAsync(userId, new string[] { "User" });

            if (!addToRolesResult.Succeeded)
            {
                ModelState.AddModelError("", "Failed to add user roles");
                return BadRequest(ModelState);
            }
            
            //Uri locationHeader = new Uri(Url.Link("GetUserById", new { id = user.Id }));

            //return Created(locationHeader, TheModelFactory.Create(user));

            return Ok();
        }

        [Authorize]
        [Route("invite")]
        [HttpPost]
        public async Task<IHttpActionResult> InviteUser(InviteUserBindingModel inviteUserModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new ApplicationUser()
            {
                UserName = inviteUserModel.Email,
                Email = inviteUserModel.Email,
                FirstName = inviteUserModel.FirstName,
                LastName = inviteUserModel.LastName,
                DisplayName = inviteUserModel.DisplayName,
                JoinDate = DateTime.Now.Date,
            };

            IdentityResult createUserResult = await this.AppUserManager.CreateAsync(user);

            if (!createUserResult.Succeeded)
            {
                return GetErrorResult(createUserResult);
            }

            string code = await this.AppUserManager.GenerateEmailConfirmationTokenAsync(user.Id);

            var callbackUrl = new Uri(Url.Link("ConfirmInviteRoute", new { userId = user.Id, code = code }));

            await this.AppUserManager.SendEmailAsync(user.Id, "Account Invite", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

            Uri locationHeader = new Uri(Url.Link("GetUserById", new { id = user.Id }));

            return Created(locationHeader, TheModelFactory.Create(user));
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("confirm-invite", Name = "ConfirmInviteRoute")]
        public async Task<IHttpActionResult> ConfirmInvite(string userId = "", string code = "")
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(code))
            {
                ModelState.AddModelError("", "User Id and Code are required");
                return BadRequest(ModelState);
            }

            IdentityResult confirmEmailResult = await this.AppUserManager.ConfirmEmailAsync(userId, code);

            if (!confirmEmailResult.Succeeded)
            {
                return GetErrorResult(confirmEmailResult);
            }

            IdentityResult addToRolesResult = await this.AppUserManager.AddToRolesAsync(userId, new string[] { "User" });

            if (!addToRolesResult.Succeeded)
            {
                ModelState.AddModelError("", "Failed to add user roles");
                return BadRequest(ModelState);
            }

            //Uri locationHeader = new Uri(Url.Link("GetUserById", new { id = user.Id }));

            //return Created(locationHeader, TheModelFactory.Create(user));

            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [Route("users")]
        public IHttpActionResult GetUsers()
        {
            return Ok(this.AppUserManager.Users.ToList().Select(u => this.TheModelFactory.Create(u)));
        }

        [Authorize(Roles = "Admin")]
        [Route("user/{id:guid}", Name = "GetUserById")]
        public async Task<IHttpActionResult> GetUser(string Id)
        {
            var user = await this.AppUserManager.FindByIdAsync(Id);

            if (user != null)
            {
                return Ok(this.TheModelFactory.Create(user));
            }

            return NotFound();

        }

        [Authorize(Roles = "Admin")]
        [Route("user/{username}")]
        public async Task<IHttpActionResult> GetUserByName(string username)
        {
            var user = await this.AppUserManager.FindByNameAsync(username);

            if (user != null)
            {
                return Ok(this.TheModelFactory.Create(user));
            }

            return NotFound();

        }

        [Authorize]
        [Route("changePassword")]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await this.AppUserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [Route("user/{id:guid}")]
        //[HttpDelete]
        public async Task<IHttpActionResult> DeleteUser(string id)
        {

            //Only SuperAdmin or Admin can delete users (Later when implement roles)

            var appUser = await this.AppUserManager.FindByIdAsync(id);

            if (appUser != null)
            {
                IdentityResult result = await this.AppUserManager.DeleteAsync(appUser);

                if (!result.Succeeded)
                {
                    return GetErrorResult(result);
                }

                return Ok();

            }

            return NotFound();

        }

        [Authorize(Roles = "Admin")]
        [Route("user/{id:guid}/roles")]
        [HttpPut]
        public async Task<IHttpActionResult> AssignRolesToUser([FromUri] string id, [FromBody] string[] rolesToAssign)
        {
            var appUser = await this.AppUserManager.FindByIdAsync(id);

            if (appUser == null)
            {
                return NotFound();
            }

            var currentRoles = await this.AppUserManager.GetRolesAsync(appUser.Id);

            var rolesNotExists = rolesToAssign.Except(this.AppRoleManager.Roles.Select(x => x.Name)).ToArray();

            if (rolesNotExists.Count() > 0)
            {

                ModelState.AddModelError("", string.Format("Roles '{0}' does not exixts in the system", string.Join(",", rolesNotExists)));
                return BadRequest(ModelState);
            }

            IdentityResult removeResult = await this.AppUserManager.RemoveFromRolesAsync(appUser.Id, currentRoles.ToArray());

            if (!removeResult.Succeeded)
            {
                ModelState.AddModelError("", "Failed to remove user roles");
                return BadRequest(ModelState);
            }

            IdentityResult addResult = await this.AppUserManager.AddToRolesAsync(appUser.Id, rolesToAssign);

            if (!addResult.Succeeded)
            {
                ModelState.AddModelError("", "Failed to add user roles");
                return BadRequest(ModelState);
            }

            return Ok();
        }
    }
}