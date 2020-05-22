using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetYouServer.Models
{
    public class MatchOpponent : DatabaseModel, IServerModel
    {
        public const ServerModel Model = ServerModel.Team;
        private const string DBTableName = "match_opponents";

        public enum Attribute { Match, Team }

        public Match Match  { get; set; }
        public Team Team    { get; set; }

        //DB Purpose - Clear before use! Add only next DBQuery related attributes 
        public List<Attribute> Attributes = new List<Attribute>();

        public ServerModel GetServerModel()
        {
            return Model;
        }

        public override DatabaseModel GetCopy()
        {
            return new MatchOpponent()
            {
                Match = Match,
                Team = Team
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
                    case Attribute.Match: attributes.Add(Attribute.Match.GetDBRepresentation(), Match == null ? NullVal : Match.ID.ToString()); break;
                    case Attribute.Team: attributes.Add(Attribute.Team.GetDBRepresentation(),   Team == null ? NullVal : Team.ID.ToString()); break;
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
                    case MatchOpponentAttributeExtensions.AttrMatch: Match = new Match() { ID = Convert.ToInt32(attribute.Value) }; break;
                    case MatchOpponentAttributeExtensions.AttrTeam:  Team = new Team() { ID = Convert.ToInt32(attribute.Value) }; break;
                    default: break;
                }
            }
        }
    }

    public static class MatchOpponentAttributeExtensions
    {
        public const string AttrMatch = "match_id";
        public const string AttrTeam = "team_id";
        public const string AttrNone = "";

        public static string GetDBRepresentation(this MatchOpponent.Attribute attribute)
        {
            switch (attribute)
            {
                case MatchOpponent.Attribute.Match: return AttrMatch;
                case MatchOpponent.Attribute.Team: return AttrTeam;
                default: return AttrNone;
            }
        }
    }
}