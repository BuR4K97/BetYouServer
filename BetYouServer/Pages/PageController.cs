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

    }
}
