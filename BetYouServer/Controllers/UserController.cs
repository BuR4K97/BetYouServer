using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using BetYouServer.Models;

namespace BetYouServer.Controllers
{
    [Route("BetYou/")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private enum SessionIdentifier { Account, UserType }

        [HttpPost("Login")]
        public ActionResult<ServerResponse> Login(Account loginInfo)
        {
            loginInfo.ID = 0;
            loginInfo.Forename = "Burak";
            loginInfo.Surname = "Mutlu";
            loginInfo.Email = "burak_mutlu_97@hotmail.com";
            loginInfo.PicLink = "https:////www.betyoupics.com//mypic.jpg";

            HttpContext.Session.SetString(SessionIdentifier.Account.ToString(), loginInfo.ID.ToString());
            HttpContext.Session.SetString(SessionIdentifier.UserType.ToString(), ServerModel.User.ToString());

            ServerResponse response = new ServerResponse();
            response.Data.Add(loginInfo.GetServerModel(), loginInfo);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }
  
    }
}
