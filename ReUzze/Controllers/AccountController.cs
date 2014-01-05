using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using ReUzze.Filters;
using ReUzze.Models;
using ReUzze.Helpers;
using App.Data.orm;
using System.Web.Routing;
using System.Net.Mail;

namespace ReUzze.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class AccountController : AdminController
    {
        public ReUzzeMembershipProvider MembershipService { get; set; }
        public ReUzzeRoleProvider AuthorizationService { get; set; }

        protected override void Initialize(RequestContext requestContext)
        {
            if (MembershipService == null)
                MembershipService = new ReUzzeMembershipProvider();
            if (AuthorizationService == null)
                AuthorizationService = new ReUzzeRoleProvider();

            base.Initialize(requestContext);
        }
        
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (MembershipService.ValidateUser(model.UserName, model.Password))
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "The username or password provided is incorrect.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model); ;
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            var model = new ReUzze.Models.RegisterViewModel
            {
                Regions = new SelectList(this.UnitOfWork.RegionRepository.Get(), "Id", "Name")
            };

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            // Check if there are roles in the database
            var roles = this.UnitOfWork.RoleRepository.Get().Count();
            if (roles == 0)
            {
                var modl = new ReUzze.Models.RegisterViewModel
                {
                    StatusMessage = "There are no roles in the database ...",
                    Regions = new SelectList(this.UnitOfWork.RegionRepository.Get(), "Id", "Name")
                };
                return View(modl);
            }

            // Check if there are regions in database
            var regions = this.UnitOfWork.RegionRepository.Get().Count();
            if (regions == 0)
            {
                var modl = new ReUzze.Models.RegisterViewModel
                {
                    StatusMessage = "There are no regions in the database ...",
                    Regions = new SelectList(this.UnitOfWork.RegionRepository.Get(), "Id", "Name")
                };
                return View(modl);
            }

            // Check if user has selected region
            if (model.SelectRegionId == 0)
            {
                var modl = new ReUzze.Models.UserViewModel
                {
                    StatusMessage = "You need to select a region!",
                    Regions = new SelectList(this.UnitOfWork.RegionRepository.Get(), "Id", "Name")
                };
                return View(modl);
            }

            // Check if username already exists
            var usrname = this.UnitOfWork.UserRepository.Get().Where(u => u.UserName == model.UserName).Count();
            if (usrname != 0)
            {
                var modl = new ReUzze.Models.RegisterViewModel
                {
                    StatusMessage = "Please choose another username. This one already exists.",
                    Regions = new SelectList(this.UnitOfWork.RegionRepository.Get(), "Id", "Name")
                };
                return View(modl);
            }

            // Check if user email already exists
            var email = this.UnitOfWork.UserRepository.Get().Where(u => u.Email == model.Email).Count();
            if (email != 0)
            {
                var modl = new ReUzze.Models.RegisterViewModel
                {
                    StatusMessage = "This email address is already registered.",
                    Regions = new SelectList(this.UnitOfWork.RegionRepository.Get(), "Id", "Name")
                };
                return View(modl);
            }

            if (!ModelState.IsValid)
            {
                model = new ReUzze.Models.RegisterViewModel
                {
                    Regions = new SelectList(this.UnitOfWork.RegionRepository.Get(), "Id", "Name")
                };

                return View(model);
            }

            else if (ModelState.IsValid)
            {
                // Attempt to register the user
                try
                {
                    MembershipService.CreateUser(model.City, model.Street, model.StreetNumber, Convert.ToDecimal(model.Latitude), Convert.ToDecimal(model.Longitude), model.SelectRegionId, model.FirstName, model.SurName, model.UserName, model.Email, model.Password);
                    FormsAuthentication.SetAuthCookie(model.UserName, false);

                    return RedirectToAction("Index", "Home");
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                }
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Disassociate(string provider, string providerUserId)
        {
            string ownerAccount = OAuthWebSecurity.GetUserName(provider, providerUserId);
            ManageMessageId? message = null;

            // Only disassociate the account if the currently logged in user is the owner
            if (ownerAccount == User.Identity.Name)
            {
                // Use a transaction to prevent the user from deleting their last login credential
                using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
                {
                    bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
                    if (hasLocalAccount || OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name).Count > 1)
                    {
                        OAuthWebSecurity.DeleteAccount(provider, providerUserId);
                        scope.Complete();
                        message = ManageMessageId.RemoveLoginSuccess;
                    }
                }
            }

            return RedirectToAction("Manage", new { Message = message });
        }

        [AllowAnonymous]
        public ActionResult Recover()
        {
            var model = new RecoverViewModel();
            return View(model);
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Recover(RecoverViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // CHECK IF THE USERS EMAIL EXISTS IN DB
                    var usar = this.UnitOfWork.UserRepository.Get().Where(u => u.Email == model.Email);
                    if (usar.FirstOrDefault() == null)
                    {
                        var modl = new ReUzze.Models.RecoverViewModel
                        {
                            StatusMessage = "E-mail adres not found in database."
                        };

                        return View(modl);
                    }

                    App.Models.User usr = this.UnitOfWork.UserRepository.Get().Where(u => u.Email == model.Email).FirstOrDefault();
                    var password = BCrypt.Net.BCrypt.HashPassword(usr.Password, usr.PasswordSalt);

                    // SETTINGS NEED TO BE ADJUSTED TO THE SERVER
                    MailMessage mail = new MailMessage();
                    SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                    mail.From = new MailAddress("info@reuzze.be");
                    mail.To.Add(model.Email);
                    mail.Subject = "Password recovery - ReUzze";
                    mail.Body = "Hi, Your password is: " + password;

                    SmtpServer.Send(mail);

                    return RedirectToAction("Index", "Home");
                }
                catch (ArgumentException ae)
                {
                    ModelState.AddModelError("", ae.Message);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public ActionResult Manage()
        {
            App.Models.User usr = this.UnitOfWork.UserRepository.Get().Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            var model = new ChangeSettingsViewModel()
            {
                UserName = usr.UserName,
                OldPassword = usr.Password,
            };

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(ChangeSettingsViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (this.UnitOfWork.UserRepository.Get().Any(user => (BCrypt.Net.BCrypt.HashPassword(model.OldPassword, user.PasswordSalt) == user.Password)))
                {
                    // Attempt to register the User
                    try
                    {
                        model.UserName = User.Identity.Name;

                        MembershipService.ChangePassword(model.UserName, model.OldPassword, model.NewPassword);

                        return RedirectToAction("Index", "Home");
                    }
                    catch (ArgumentException ae)
                    {
                        ModelState.AddModelError("", ae.Message);
                    }
                }

                else
                {
                    model.StatusMessage = "Your old password isn't correct";
                    return View(model);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }

        internal class ExternalLoginResult : ActionResult
        {
            public ExternalLoginResult(string provider, string returnUrl)
            {
                Provider = provider;
                ReturnUrl = returnUrl;
            }

            public string Provider { get; private set; }
            public string ReturnUrl { get; private set; }

            public override void ExecuteResult(ControllerContext context)
            {
                OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);
            }
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}
