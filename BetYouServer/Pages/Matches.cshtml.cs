using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BetYouServer.Models;

namespace BetYouServer.Pages
{
    public class MatchesModel : PageModel
    {
        public List<(Match, Team, Team)> Matches;

        public void OnGet()
        {
            Matches = Globals.PageController.RetrieveMatches(HttpContext);
        }
    }
}