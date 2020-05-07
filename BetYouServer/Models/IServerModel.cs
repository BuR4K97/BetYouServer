using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetYouServer.Models
{
    public enum ServerModel { Account, User, Admin }

    public interface IServerModel
    {

        ServerModel GetServerModel();

    }
}
