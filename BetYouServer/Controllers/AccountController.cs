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

        public ServerException GetAccountDetails(Account account)
        {
            ExecutionResult result;
            Account tempAcc = new Account(); tempAcc.Attributes.AddRange(new List<Account.Attribute>()
            {
                Account.Attribute.Username,
                Account.Attribute.Forename,
                Account.Attribute.Surname,
                Account.Attribute.Email,
                Account.Attribute.PicLink
            });
            account.Attributes.Add(Account.Attribute.ID);
            result = DBController.SelectData(tempAcc, account, new List<DatabaseQuery.Condition>() { DatabaseQuery.Condition.Equal });
            account.Attributes.Clear(); tempAcc.Attributes.Clear();

            if (result.Data.Count == 0) return ServerException.InvalidAccountID;

            tempAcc = result.Data.First() as Account;
            account.Username = tempAcc.Username;
            account.Forename = tempAcc.Forename;
            account.Surname = tempAcc.Surname;
            account.Email = tempAcc.Email;
            account.PicLink = tempAcc.PicLink;
            return ServerException.None;
        }

        public (User, ServerException) GetAccountUser(Account account)
        {
            ExecutionResult result;
            User user = new User(); user.Attributes.AddRange(new List<User.Attribute>()
            {
                User.Attribute.BetPermission,
                User.Attribute.SocialPermission,
                User.Attribute.Balance,
                User.Attribute.VirtualBalance,
                User.Attribute.Rating,
                User.Attribute.Member
            });
            User tempUser = new User() { Account = account }; tempUser.Attributes.Add(User.Attribute.Account);
            result = DBController.SelectData(user, tempUser, new List<DatabaseQuery.Condition>() { DatabaseQuery.Condition.Equal });
            user.Attributes.Clear(); tempUser.Attributes.Clear();

            if (result.Data.Count == 0) return (null, ServerException.InvalidAccountID);

            user = result.Data.First() as User;
            user.Account = tempUser.Account;
            return (user, ServerException.None);
        }

        public (Admin, ServerException) GetAccountAdmin(Account account)
        {
            ExecutionResult result;
            Admin admin = new Admin(); admin.Attributes.Add(Admin.Attribute.Nickname);
            Admin tempAdmin = new Admin() { Account = account }; tempAdmin.Attributes.Add(Admin.Attribute.Account);
            result = DBController.SelectData(admin, tempAdmin, new List<DatabaseQuery.Condition>() { DatabaseQuery.Condition.Equal });
            admin.Attributes.Clear(); tempAdmin.Attributes.Clear();

            if (result.Data.Count == 0) return (null, ServerException.InvalidAccountID);

            admin = result.Data.First() as Admin;
            admin.Account = tempAdmin.Account;
            return (admin, ServerException.None);
        }

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

            Admin admin = new Admin(); admin.Attributes.Add(Admin.Attribute.Nickname);
            Admin tempAdmin = new Admin() { Account = login }; tempAdmin.Attributes.Add(Admin.Attribute.Account);
            result = DBController.SelectData(admin, tempAdmin, new List<DatabaseQuery.Condition>() { DatabaseQuery.Condition.Equal });
            admin.Attributes.Clear(); tempAdmin.Attributes.Clear();

            admin = result.Data.First() as Admin;
            admin.Account = tempAdmin.Account;
            return (admin, ServerException.None);
        }

        public (User, ServerException) RegisterUser(Account register)
        {
            ServerException exception = Register(register);
            if (exception != ServerException.None) return (null, exception);

            User user = new User() { Account = register };
            user.Attributes.Add(User.Attribute.Account);
            DBController.InsertData(user);
            user.Attributes.Clear();
            return (user, ServerException.None);
        }

        public ServerException RegisterAdmin(Account register, Admin admin)
        {
            ExecutionResult result;
            Admin temp = new Admin(); temp.Attributes.Add(Admin.Attribute.Account);
            admin.Attributes.Add(Admin.Attribute.Nickname);
            result = DBController.SelectData(temp, admin, new List<DatabaseQuery.Condition>() { DatabaseQuery.Condition.Equal });
            admin.Attributes.Clear(); temp.Attributes.Clear();

            if (result.Data.Count > 0)
            {
                return ServerException.RegisteredNickname;
            }
            
            ServerException exception = Register(register);
            if (exception != ServerException.None) return exception;

            admin.Account = register;
            admin.Attributes.AddRange(new List<Admin.Attribute>() { Admin.Attribute.Account, Admin.Attribute.Nickname } );
            DBController.InsertData(admin);
            admin.Attributes.Clear();
            return ServerException.None;
        }

        public ServerException UpdateAccount(Account update)
        {
            ExecutionResult result;
            List<DatabaseQuery.Helper> setters = new List<DatabaseQuery.Helper>();
            if (!String.IsNullOrEmpty(update.Password)) { update.Attributes.Add(Account.Attribute.Password); setters.Add(DatabaseQuery.Helper.Equal); }
            if (!String.IsNullOrEmpty(update.Forename)) { update.Attributes.Add(Account.Attribute.Forename); setters.Add(DatabaseQuery.Helper.Equal); }
            if (!String.IsNullOrEmpty(update.Surname)) { update.Attributes.Add(Account.Attribute.Surname); setters.Add(DatabaseQuery.Helper.Equal); }
            if (!String.IsNullOrEmpty(update.Email)) { update.Attributes.Add(Account.Attribute.Email); setters.Add(DatabaseQuery.Helper.Equal); }
            if (!String.IsNullOrEmpty(update.PicLink)) { update.Attributes.Add(Account.Attribute.PicLink); setters.Add(DatabaseQuery.Helper.Equal); }

            Account tempAcc = new Account() { ID = update.ID }; tempAcc.Attributes.Add(Account.Attribute.ID);
            result = DBController.UpdateData(update, setters, tempAcc, new List<DatabaseQuery.Condition>() { DatabaseQuery.Condition.Equal });
            update.Attributes.Clear(); tempAcc.Attributes.Clear();

            if (result.Stat == ExecutionResult.Status.Fail)
            {
                if (result.FailInfo.Contains(Account.Attribute.Email.GetDBRepresentation())) return ServerException.RegisteredEmail;
            }
            return ServerException.None;
        }

        private ServerException Register(Account register)
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
                if (result.FailInfo.Contains(Account.Attribute.Username.GetDBRepresentation())) return ServerException.RegisteredUsername;
                if (result.FailInfo.Contains(Account.Attribute.Email.GetDBRepresentation())) return ServerException.RegisteredEmail;
            }

            Account tempAcc = new Account(); tempAcc.Attributes.Add(Account.Attribute.ID);
            register.Attributes.Add(Account.Attribute.Username);
            result = DBController.SelectData(tempAcc, register, new List<DatabaseQuery.Condition>() { DatabaseQuery.Condition.Equal });
            register.Attributes.Clear(); tempAcc.Attributes.Clear();

            tempAcc = result.Data.First() as Account;
            register.ID = tempAcc.ID;
            return ServerException.None;
        }
    }
}
