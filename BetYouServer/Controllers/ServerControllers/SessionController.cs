using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using BetYouServer.Models;

namespace BetYouServer.Controllers
{
    public partial class RequestController : ControllerBase
    {
        private enum SessionIdentifier { Account, Authorization }
        private enum Authorization { Unauthorized, User, Admin }

        private void CreateSession(Account account, Actor actor)
        {
            HttpContext.Session.SetString(SessionIdentifier.Account.ToString(), account.ID.ToString());
            if (actor is User)
            {
                HttpContext.Session.SetString(SessionIdentifier.Authorization.ToString(), Authorization.User.ToString());
            }
            else
            {
                HttpContext.Session.SetString(SessionIdentifier.Authorization.ToString(), Authorization.Admin.ToString());
            }
        }

        private Account GetSessionAccount()
        {
            string accountID = HttpContext.Session.GetString(SessionIdentifier.Account.ToString());
            if (String.IsNullOrEmpty(accountID)) return null;
            return new Account() { ID = Convert.ToInt32(accountID) };
        }

        private Authorization GetSessionAuthorization()
        {
            string authorization = HttpContext.Session.GetString(SessionIdentifier.Authorization.ToString());
            if (String.IsNullOrEmpty(authorization)) return Authorization.Unauthorized;
            return Enum.Parse<Authorization>(authorization);
        }

    }
}
