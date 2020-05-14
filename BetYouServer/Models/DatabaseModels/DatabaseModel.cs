using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetYouServer.Models
{
    public abstract class DatabaseModel
    {
        public const string NullVal = "NULL";
        protected const char Quote = '\'';

        public abstract DatabaseModel GetCopy();
        public abstract string GetTableName();
        public abstract Dictionary<string, string> GetAttributes();
        public abstract void SetAttributes(Dictionary<string, string> attributes);
        
    }
}
