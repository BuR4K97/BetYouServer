using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BetYouServer.Configurations;
using BetYouServer.Models;


namespace BetYouServer.Controllers
{
    public class AccountController
    {
        private readonly DatabaseController DBController = Configuration.GetService<DatabaseController>();

        public (Actor, ServerException) Login(Account login)
        {
            ExecutionResult result;
            Account tempAcc = new Account(); tempAcc.Attributes.AddRange(new List<Account.Attribute>()
            {
                Account.Attribute.ID,
                Account.Attribute.Password,
                Account.Attribute.Forename,
                Account.Attribute.Surname,
                Account.Attribute.Email,
                Account.Attribute.PicLink
            });
            login.Attributes.Add(Account.Attribute.Username);
            result = DBController.SelectData(tempAcc, login, new List<DatabaseQuery.Condition>() { DatabaseQuery.Condition.Equal });
            login.Attributes.Clear(); tempAcc.Attributes.Clear();

            if (result.Data.Count == 0) return (null, ServerException.UnknownUsername);
            tempAcc = result.Data.First() as Account;
            if (tempAcc.Password != login.Password) return (null, ServerException.InvalidLoginCredentials);

            login.ID = tempAcc.ID;
            login.Forename = tempAcc.Forename;
            login.Surname = tempAcc.Surname;
            login.Email = tempAcc.Email;
            login.PicLink = tempAcc.PicLink;

            User user = new User(); user.Attributes.AddRange(new List<User.Attribute>()
            {
                User.Attribute.BetPermission,
                User.Attribute.SocialPermission,
                User.Attribute.Balance,
                User.Attribute.VirtualBalance,
                User.Attribute.Rating,
                User.Attribute.Member
            });
            User tempUser = new User() { Account = login }; tempUser.Attributes.Add(User.Attribute.Account);
            result = DBController.SelectData(user, tempUser, new List<DatabaseQuery.Condition>() { DatabaseQuery.Condition.Equal });
            user.Attributes.Clear(); tempUser.Attributes.Clear();

            if (result.Data.Count > 0)
            {
                user = result.Data.First() as User;
                user.Account = tempUser.Account;
                return (user, ServerException.None);
            }
            //Admin admin = new Admin();
            return (null, ServerException.None);
        }

        public (User, ServerException) RegisterUser(Account register)
        {
            ExecutionResult result;
            register.Attributes.AddRange(new List<Account.Attribute>()
            {
                Account.Attribute.Username,
                Account.Attribute.Password,
                Account.Attribute.Forename,
                Account.Attribute.Surname,
                Account.Attribute.Email
            });
            result = DBController.InsertData(register);
            register.Attributes.Clear();

            if (result.Stat == ExecutionResult.Status.Fail)
            {
                if(result.FailInfo.Contains(Account.Attribute.Username.GetDBRepresentation())) return (null, ServerException.RegisteredUsername);
                if (result.FailInfo.Contains(Account.Attribute.Email.GetDBRepresentation())) return (null, ServerException.RegisteredEmail);
            }

            Account tempAcc = new Account(); tempAcc.Attributes.Add(Account.Attribute.ID);
            register.Attributes.Add(Account.Attribute.Username);
            result = DBController.SelectData(tempAcc, register, new List<DatabaseQuery.Condition>() { DatabaseQuery.Condition.Equal });
            register.Attributes.Clear(); tempAcc.Attributes.Clear();

            tempAcc = result.Data.First() as Account;
            register.ID = tempAcc.ID;

            User user = new User() { Account = register };
            user.Attributes.Add(User.Attribute.Account);
            DBController.InsertData(user);
            user.Attributes.Clear();
            return (user, ServerException.None);
        }

        //public bool RegisterAdmin(Account account, Admin admin) {}

    }
}
