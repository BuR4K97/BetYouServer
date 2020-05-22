using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using BetYouServer.Models;
using BetYouServer.Configurations;

namespace BetYouServer.Controllers
{
    public partial class RequestController : ControllerBase
    {
        private static readonly SportController SportController = Configuration.GetService<SportController>();
        
        [HttpGet(RequestControllerConfiguration.UserRequest.RetrieveMatches)]
        public ServerActionResult RetrieveMatches()
        {
            ServerResponse response = new ServerResponse();
            List<(Match, Team, Team)> matches = SportController.GetMatches();
            foreach ((Match, Team, Team) match in matches)
            {
                response.InsertData(match.Item1.GetServerModel(), match.Item1);
                response.InsertData(match.Item2.GetServerModel(), match.Item2);
                response.InsertData(match.Item3.GetServerModel(), match.Item3);
            }
            return new ServerActionResult(ServerActionResult.Status.Ok, response);
        }

    }
}
