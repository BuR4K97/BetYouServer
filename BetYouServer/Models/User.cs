using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetYouServer.Models
{
    public class User : IServerModel
    {
        public const ServerModel Model = ServerModel.USER;
        public enum Membership { BRONZE, SILVER, GOLD }
       
        public bool BetPermission;
        public bool SocialPermission;
        public float Balance;
        public float VirtualBalance;
        public int Rating;
        public Membership Membership;

        public ServerModel GetServerModel()
        {
            return Model;
        }
    }

    public class MembershipExtensions
    {

        public Member

    }
}
