using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetYouServer.Models
{
    public enum ServerException
    {
        None, UnknownUsername, InvalidLoginCredentials, RegisteredUsername, RegisteredEmail, RegisteredNickname, InvalidAccountID
    }

    public class ServerResponse
    {
        public List<KeyValuePair<ServerModel, IServerModel>> Data = new List<KeyValuePair<ServerModel, IServerModel>>();
        public ServerException Exception = ServerException.None;

        public void InsertData(ServerModel type, IServerModel model)
        {
            Data.Add(new KeyValuePair<ServerModel, IServerModel>(type, model));
        }
    }

}
