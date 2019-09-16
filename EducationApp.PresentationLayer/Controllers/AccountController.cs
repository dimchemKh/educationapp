using EducationApp.BusinessLayer.Services.Interfaces;
using EducationApp.DataAccessLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EducationApp.PresentationLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Register()
        //{
            //if (ModelState.IsValid)
            //{
            //    var user = new ApplicationUser {  };
            //    var result = await UserManager<ApplicationUser>.CreateAsync(user, );
            //    if (result.Succeeded)
            //    {
            //        await SignInManager<ApplicationUser>.SignInAsync(user, isPersistent: false, rememberBrowser: false);

            //        string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
            //        var callbackUrl = Url.Action("ConfirmEmail", "Account",
            //           new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
            //        await UserManager.SendEmailAsync(user.Id,
            //           "Confirm your account", "Please confirm your account by clicking <a href=\""
            //           + callbackUrl + "\">here</a>");

            //        return RedirectToAction("Index", "Home");
            //    }
            //    AddErrors(result);
            //}

            //// If we got this far, something failed, redisplay form
            //return View(model);
        //}
    }
}
