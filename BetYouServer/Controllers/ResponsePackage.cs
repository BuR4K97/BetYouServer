using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BetYouServer.Models;

namespace BetYouServer.Controllers
{
    public class ResponsePackage
    {
        public ResponseData response_data;
        public ServerException server_exception;
    }

    public class ResponseData
    {
        public Dictionary<, ServerModel> data;
    }

    public enum ServerException { NONE }

}
