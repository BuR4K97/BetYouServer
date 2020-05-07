using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BetYouServer.Models;

namespace BetYouServer.Controllers
{
    public enum ServerException { NONE }

    public class ServerResponse
    {
        public Dictionary<ServerModel, IServerModel> Data = new Dictionary<ServerModel, IServerModel>();
        public ServerException Exception = ServerException.NONE;
    }

}
