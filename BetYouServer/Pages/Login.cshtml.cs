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
            Actor actor; (actor, Exception) = Globals.PageController.Login(HttpContext, Account);
            if (Exception == ServerException.None)
            {
                if(actor is User) ViewData.Add(Globals.AuthorizationKey, Authorization.User);
                else ViewData.Add(Globals.AuthorizationKey, Authorization.Admin);
                ViewData.Add(Globals.ActorKey, actor);
                return Redirect("~/Home");
            }
            return Page();
        }
    }
}