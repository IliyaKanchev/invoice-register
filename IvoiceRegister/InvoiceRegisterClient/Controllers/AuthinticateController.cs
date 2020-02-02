using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using InvoiceRegisterClient.Helpers;
using InvoiceRegisterClient.Models;
using InvoiceRegisterClient.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace InvoiceRegisterClient.Controllers
{
    [Route("/[controller]")]
    public class AuthenticateController : Controller
    {
        private readonly IAuthenticateService _authService;

        public AuthenticateController(IAuthenticateService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult AuthenticateIndex()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Authenticate(AuthenticateViewModel authenticate)
        {
            User user = _authService.Authenticate(authenticate);

            if (user != null)
            {
                HttpContext.Session.SetString("InvoiceRegisterJWToken", user.Token);
                return Redirect("~/");
            }

            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");

            return View("AuthenticateIndex", authenticate);
        }
    }
}
