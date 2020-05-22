using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BetYouServer.Pages
{
    public class LogoutModel : PageModel
    {
        public ActionResult OnGet()
        {
            Globals.PageController.Logout(HttpContext);
            return Redirect("~/Home");
        }
    }
}