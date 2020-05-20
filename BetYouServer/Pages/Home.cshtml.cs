using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BetYouServer.Controllers;

namespace BetYouServer.Pages
{
    public class HomeModel : PageModel
    {
        public ActionResult OnGet()
        {
            if (Globals.SessionController.GetSessionAuthorization(HttpContext) == Authorization.Unauthorized)
            {
                return Redirect("~/Login");
            }
            return Page();
        }
    }
}