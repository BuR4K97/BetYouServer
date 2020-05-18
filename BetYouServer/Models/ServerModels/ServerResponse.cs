using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetYouServer.Models
{
    public enum ServerException { None, UnknownUsername, InvalidLoginCredentials, RegisteredUsername, RegisteredEmail }

    public class ServerResponse
    {
        public Dictionary<ServerModel, IServerModel> Data = new Dictionary<ServerModel, IServerModel>();
        public ServerException Exception = ServerException.None;
    }

}
