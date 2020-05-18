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
    [Route("BetYou/")]
    [ApiController]
    public partial class RequestController : ControllerBase
    {
        private static readonly AccountController AccountController = Configuration.GetService<AccountController>();

        [HttpPost("Register")]
        public ActionResult<ServerResponse> Register(Account register)
        {
            ServerResponse response = new ServerResponse();
            (User user, ServerException exception) = AccountController.RegisterUser(register);
            if (exception == ServerException.None)
            {
                CreateSession(register, user);
                response.Data.Add(user.GetServerModel(), user);
            }
            else
            {
                response.Exception = exception;
            }
            return Ok(response);
        }

        [HttpPost("Login")]
        public ActionResult<ServerResponse> Login(Account login)
        {
            ServerResponse response = new ServerResponse();
            (Actor actor, ServerException exception) = AccountController.Login(login);
            if (exception == ServerException.None)
            {
                CreateSession(login, actor);
                response.Data.Add(actor.GetServerModel(), actor);
            }
            else
            {
                response.Exception = exception;
            }
            return Ok(response);
        }

    }
}
