using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using BetYouServer.Configurations;
using BetYouServer.Models;

namespace BetYouServer.Controllers
{
    public partial class RequestController : ControllerBase
    {

        [HttpPost(RequestControllerConfiguration.AdminRequest.CreateAdmin)]
        public ServerActionResult CreateAdmin(Admin create)
        {
            if (SessionController.GetSessionAuthorization(HttpContext) != Authorization.Admin)
            {
                return new ServerActionResult(ServerActionResult.Status.Unauthorized);
            }

            ServerResponse response = new ServerResponse();
            response.Exception = AccountController.RegisterAdmin(create.Account, create);
            return new ServerActionResult(ServerActionResult.Status.Ok, response);
        }

    }
}
