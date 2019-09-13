using EducationApp.BusinessLayer.Services.Interfaces;
using EducationApp.DataAccessLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EducationApp.PresentationLayer.Controllers
{
    public class AccountController : Controller
    {
        private IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        [HttpGet]
        public void Index()
        {

        }

        [HttpPost]
        public void Register()
        {
            //_accountService.RegisterAsync("Firest", "Last", "Pass");
        }

        public void Login()
        {

        }
    }
}
