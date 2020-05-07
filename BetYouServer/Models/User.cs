using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetYouServer.Models
{
    public class User : IServerModel
    {
        public const ServerModel Model = ServerModel.User;
        public enum Membership { BRONZE, SILVER, GOLD }
       
        public bool BetPermission;
        public bool SocialPermission;
        public float Balance;
        public float VirtualBalance;
        public int Rating;
        public Membership Member;

        public ServerModel GetServerModel()
        {
            return Model;
        }
    }

}
