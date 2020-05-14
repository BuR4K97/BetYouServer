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
    public partial class RequestController : ControllerBase
    {

        private enum SessionIdentifier { Account, UserType }
        private static readonly Type _dbController = typeof(DBConnectionController);

        [HttpPost("Register")]
        public ActionResult<ServerResponse> Register(Account register)
        {
            DBConnectionController dbController = (DBConnectionController) HttpContext.RequestServices.GetService(_dbController);
            DatabaseConnection dbConn = dbController.RetrieveDBConnection();

            register.Attributes.AddRange(new List<Account.Attribute>()
            {
                Account.Attribute.Username,
                Account.Attribute.Password,
                Account.Attribute.Forename,
                Account.Attribute.Surname,
                Account.Attribute.Email
            });
            dbConn.ExecuteQuery(DatabaseQuery.ConstructInsertStatement(register)); register.Attributes.Clear();

            register.Attributes.Add(Account.Attribute.Username);
            Account temp = new Account(); temp.Attributes.Add(Account.Attribute.ID);
            DatabaseQuery query = DatabaseQuery.ConstructSelectStatement(temp, register, new List<DatabaseQuery.Condition>() { DatabaseQuery.Condition.Equal });
            dbConn.ExecuteQuery(query);
            temp = (Account) query.Results.First();
            register.ID = temp.ID;
                
            User user = new User() { Account = register };
            user.Attributes.Add(Models.User.Attribute.Account);
            dbConn.ExecuteQuery(DatabaseQuery.ConstructInsertStatement(user)); user.Attributes.Clear();
            dbController.ReleaseDBConnection(dbConn);

            HttpContext.Session.SetString(SessionIdentifier.Account.ToString(), register.ID.ToString());
            HttpContext.Session.SetString(SessionIdentifier.UserType.ToString(), ServerModel.User.ToString());

            ServerResponse response = new ServerResponse();
            response.Data.Add(register.GetServerModel(), register);
            return Ok(response);
        }

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

    }
}
