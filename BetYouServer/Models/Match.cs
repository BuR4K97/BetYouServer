using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetYouServer.Models
{
    public class Match : DatabaseModel, IServerModel
    {
        public const ServerModel Model = ServerModel.Match;
        private const string DBTableName = "match_betyou";

        public enum Attribute { ID, Date, Result }

        public int ID { get; set; }
        public DateTime Date { get; set; }
        public string Result { get; set; }

        //DB Purpose - Clear before use! Add only next DBQuery related attributes 
        public List<Attribute> Attributes = new List<Attribute>();

        public ServerModel GetServerModel()
        {
            return Model;
        }

        public override DatabaseModel GetCopy()
        {
            return new Match()
            {
                ID = ID,
                Date = Date,
                Result = Result
            };
        }

        public override string GetTableName()
        {
            return DBTableName;
        }

        public override Dictionary<string, string> GetAttributes()
        {
            Attributes.Sort();
            Dictionary<string, string> attributes = new Dictionary<string, string>();
            foreach (Attribute attribute in Attributes)
            {
                switch (attribute)
                {
                    case Attribute.ID:      attributes.Add(Attribute.ID.GetDBRepresentation(),      ID.ToString());                                     break;
                    case Attribute.Date:    attributes.Add(Attribute.Date.GetDBRepresentation(),    Date == null ? NullVal : Date.ToString());          break;
                    case Attribute.Result:  attributes.Add(Attribute.Result.GetDBRepresentation(),  Result == null ? NullVal : Quote + Result + Quote); break;
                    default: break;
                }
            }
            return attributes;
        }

        public override void SetAttributes(Dictionary<string, string> attributes)
        {

            foreach (KeyValuePair<string, string> attribute in attributes)
            {
                switch (attribute.Key)
                {
                    case MatchAttributeExtensions.AttrID:       ID = Convert.ToInt32(attribute.Value);                                          break;
                    case MatchAttributeExtensions.AttrDate:     Date = Convert.ToDateTime(attribute.Value);                                     break;
                    case MatchAttributeExtensions.AttrResult:   Result = attribute.Value.Equals(NullVal) ? null : attribute.Value.Trim(Quote);  break;
                    default: break;
                }
            }
        }
    }

    public static class MatchAttributeExtensions
    {
        public const string AttrID = "id";
        public const string AttrDate = "date_match";
        public const string AttrResult = "result_match";
        public const string AttrNone = "";

        public static string GetDBRepresentation(this Match.Attribute attribute)
        {
            switch (attribute)
            {
                case Match.Attribute.ID:        return AttrID;
                case Match.Attribute.Date:      return AttrDate;
                case Match.Attribute.Result:    return AttrResult;
                default: return AttrNone;
            }
        }
    }
}