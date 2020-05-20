using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BetYouServer.Configurations;
using BetYouServer.Controllers;

namespace BetYouServer.Pages
{
    public class Globals
    {
        public const string InitialRequestKey   = "InitialRequest";
        public const string AuthorizationKey    = "Authorization";
        public const string ActorKey            = "Actor";

        public static readonly SessionController SessionController = Configuration.GetService<SessionController>();
        public static readonly PageController PageController = Configuration.GetService<PageController>();
    }
}
