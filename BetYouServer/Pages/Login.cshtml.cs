using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BetYouServer.Controllers;
using BetYouServer.Models;

namespace BetYouServer.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty] public Account Account { get; set; } = new Account();
        public ServerException Exception = ServerException.None;

        public ActionResult OnGet()
        {
            if (Globals.SessionController.GetSessionAuthorization(HttpContext) != Authorization.Unauthorized)
            {
                return Redirect("~/Home");
            }
            return Page();
        }

        public ActionResult OnPost()
        {
            if (String.IsNullOrEmpty(Account.Username) || String.IsNullOrEmpty(Account.Password)) return Page();

            Actor actor; (actor, Exception) = Globals.PageController.Login(HttpContext, Account);
            if (Exception == ServerException.None)
            {
                return Redirect("~/Home");
            }
            return Page();
        }
    }
}