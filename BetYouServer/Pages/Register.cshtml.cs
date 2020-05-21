using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BetYouServer.Models;

namespace BetYouServer.Pages
{
    public class RegisterModel : PageModel
    {
        [BindProperty] public Account Account { get; set; } = new Account();
        public ServerException Exception;

        public void OnGet()
        {

        }

        public ActionResult OnPost()
        {
            if (String.IsNullOrEmpty(Account.Username) || String.IsNullOrEmpty(Account.Password) || String.IsNullOrEmpty(Account.Forename) 
                || String.IsNullOrEmpty(Account.Surname) || String.IsNullOrEmpty(Account.Email)) return Page();

            User user;
            (user, Exception) = Globals.PageController.Register(HttpContext, Account);
            if (Exception == ServerException.None)
            {
                return Redirect("~/Home");
            }
            return Page();
        }
    }
}