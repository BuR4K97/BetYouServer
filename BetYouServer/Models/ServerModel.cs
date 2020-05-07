using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetYouServer.Models
{

    public enum ServerModel { ACCOUNT, USER }

    public interface IServerModel
    {

        ServerModel GetServerModel();

    }
}
