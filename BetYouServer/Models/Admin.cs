using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetYouServer.Models
{
    public class Admin : Actor
    {
        public override Dictionary<string, string> GetAttributes()
        {
            throw new NotImplementedException();
        }

        public override DatabaseModel GetCopy()
        {
            throw new NotImplementedException();
        }

        public override ServerModel GetServerModel()
        {
            throw new NotImplementedException();
        }

        public override string GetTableName()
        {
            throw new NotImplementedException();
        }

        public override void SetAttributes(Dictionary<string, string> attributes)
        {
            throw new NotImplementedException();
        }
    }
}
