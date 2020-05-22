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
    [Route(RequestControllerConfiguration.Route)]
    [ApiController]
    public partial class RequestController : ControllerBase
    {
        private static readonly SessionController SessionController = Configuration.GetService<SessionController>();
        private static readonly AccountController AccountController = Configuration.GetService<AccountController>();

        [HttpGet(RequestControllerConfiguration.Request.Hello)]
        public ServerActionResult Hello()
        {
            Authorization authorization = SessionController.GetSessionAuthorization(HttpContext);
            if (authorization == Authorization.Unauthorized) return new ServerActionResult(ServerActionResult.Status.Unauthorized);

            ServerResponse response = new ServerResponse();
            Account sessionAccount = SessionController.GetSessionAccount(HttpContext);
            AccountController.GetAccountDetails(sessionAccount);
            if (authorization == Authorization.User)
            {
                (User user, ServerException exception) = AccountController.GetAccountUser(sessionAccount);
                response.InsertData(user.GetServerModel(), user);
            }
            else
            {
                (Admin admin, ServerException exception) = AccountController.GetAccountAdmin(sessionAccount);
                response.InsertData(admin.GetServerModel(), admin);
            }
            return new ServerActionResult(ServerActionResult.Status.Ok, response);
        }

        [HttpPost(RequestControllerConfiguration.Request.Register)]
        public ServerActionResult Register(Account register)
        {
            ServerResponse response = new ServerResponse();
            (User user, ServerException exception) = AccountController.RegisterUser(register);
            if (exception == ServerException.None)
            {
                SessionController.CreateSession(HttpContext, register, user);
                response.InsertData(user.GetServerModel(), user);
            }
            else
            {
                response.Exception = exception;
            }
            return new ServerActionResult(ServerActionResult.Status.Ok, response);
        }

        [HttpPost(RequestControllerConfiguration.Request.Login)]
        public ServerActionResult Login(Account login)
        {
            ServerResponse response = new ServerResponse();
            (Actor actor, ServerException exception) = AccountController.Login(login);
            if (exception == ServerException.None)
            {
                SessionController.CreateSession(HttpContext, login, actor);
                response.InsertData(actor.GetServerModel(), actor);
            }
            else
            {
                response.Exception = exception;
            }
            return new ServerActionResult(ServerActionResult.Status.Ok, response);
        }

        [HttpPost(RequestControllerConfiguration.Request.UpdateAccount)]
        public ServerActionResult UpdateAccount(Account update)
        {
            if (SessionController.GetSessionAuthorization(HttpContext) == Authorization.Unauthorized)
            {
                return new ServerActionResult(ServerActionResult.Status.Unauthorized);
            }

            ServerResponse response = new ServerResponse();
            update.ID = SessionController.GetSessionAccount(HttpContext).ID;
            response.Exception = AccountController.UpdateAccount(update);
            return new ServerActionResult(ServerActionResult.Status.Ok, response);
        }

        [HttpGet(RequestControllerConfiguration.Request.Logout)]
        public ServerActionResult Logout()
        {
            if (SessionController.GetSessionAuthorization(HttpContext) == Authorization.Unauthorized) return new ServerActionResult(ServerActionResult.Status.Unauthorized);

            SessionController.TerminateSession(HttpContext);
            return new ServerActionResult(ServerActionResult.Status.Ok);
        }

    }
}
