using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using BetYouServer.Models;

namespace BetYouServer.Controllers
{
    public enum Authorization { Unauthorized, User, Admin }

    public class SessionController
    {
        private enum SessionIdentifier { Account, Authorization }

        public void CreateSession(HttpContext context, Account account, Actor actor)
        {
            context.Session.SetString(SessionIdentifier.Account.ToString(), account.ID.ToString());
            if (actor is User)
            {
                context.Session.SetString(SessionIdentifier.Authorization.ToString(), Authorization.User.ToString());
            }
            else
            {
                context.Session.SetString(SessionIdentifier.Authorization.ToString(), Authorization.Admin.ToString());
            }
        }

        public Account GetSessionAccount(HttpContext context)
        {
            string accountID = context.Session.GetString(SessionIdentifier.Account.ToString());
            if (String.IsNullOrEmpty(accountID)) return null;
            return new Account() { ID = Convert.ToInt32(accountID) };
        }

        public Authorization GetSessionAuthorization(HttpContext context)
        {
            string authorization = context.Session.GetString(SessionIdentifier.Authorization.ToString());
            if (String.IsNullOrEmpty(authorization)) return Authorization.Unauthorized;
            return Enum.Parse<Authorization>(authorization);
        }

        public void TerminateSession(HttpContext context)
        {
            context.Session.Remove(SessionIdentifier.Account.ToString());
            context.Session.Remove(SessionIdentifier.Authorization.ToString());
        }
    }
}
