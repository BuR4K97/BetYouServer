using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using BetYouServer.Controllers;
using BetYouServer.Models;

namespace BetYouServer.Pages
{
    public class PageController
    {

        public (Actor, ServerException) Login(HttpContext context, Account login)
        {
            ControllerContext controllerContext = new ControllerContext() { HttpContext = context };
            RequestController requestController = new RequestController();
            requestController.ControllerContext = controllerContext;

            ServerActionResult action = requestController.Login(login);
            ServerResponse response = action.Response;

            Actor actor = null;
            if (response.Exception == ServerException.None)
            {
                KeyValuePair<ServerModel, IServerModel> model = response.Data.First();
                actor = model.Value as Actor;
            }
            return (actor, response.Exception);
        }

        public (User, ServerException) Register(HttpContext context, Account register)
        {
            ControllerContext controllerContext = new ControllerContext() { HttpContext = context };
            RequestController requestController = new RequestController();
            requestController.ControllerContext = controllerContext;

            ServerActionResult action = requestController.Register(register);
            ServerResponse response = action.Response;

            User user = null;
            if (response.Exception == ServerException.None)
            {
                KeyValuePair<ServerModel, IServerModel> model = response.Data.First();
                user = model.Value as User;
            }
            return (user, response.Exception);
        }

        public List<(Match, Team, Team)> RetrieveMatches(HttpContext context)
        {
            ControllerContext controllerContext = new ControllerContext() { HttpContext = context };
            RequestController requestController = new RequestController();
            requestController.ControllerContext = controllerContext;

            ServerActionResult action = requestController.RetrieveMatches();
            ServerResponse response = action.Response;

            List<(Match, Team, Team)> matches = new List<(Match, Team, Team)>();
            for(int i = 0; i < response.Data.Count; i += 3)
            {
                matches.Add((response.Data.ElementAt(i).Value as Match, response.Data.ElementAt(i + 1).Value as Team , response.Data.ElementAt(i + 2).Value as Team));
            }
            return matches;
        }

        public void Logout(HttpContext context)
        {
            ControllerContext controllerContext = new ControllerContext() { HttpContext = context };
            RequestController requestController = new RequestController();
            requestController.ControllerContext = controllerContext;
            requestController.Logout();
        }

    }
}
