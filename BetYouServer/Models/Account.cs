using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetYouServer.Models
{
    public class Account : IServerModel
    {
        public const ServerModel Model = ServerModel.ACCOUNT;

        public int ID;
        public string Username;
        public string Forename;
        public string Surname;
        public string Email;
        public string PicLink;

        public ServerModel GetServerModel()
        {
            return Model;
        }

    }
}