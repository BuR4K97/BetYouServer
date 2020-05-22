using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetYouServer.Models
{
    public class Team : DatabaseModel, IServerModel
    {
        public const ServerModel Model = ServerModel.Team;
        private const string DBTableName = "team";

        public enum Attribute { ID, Name, City, FoundationDate, StarAmount, CoupeAmount, Info }

        public int ID                   { get; set; }
        public string Name              { get; set; }
        public string City              { get; set; }
        public DateTime FoundationDate  { get; set; }
        public int StarAmount           { get; set; }
        public int CoupeAmount          { get; set; }
        public string Info              { get; set; }

        //DB Purpose - Clear before use! Add only next DBQuery related attributes 
        public List<Attribute> Attributes = new List<Attribute>();

        public ServerModel GetServerModel()
        {
            return Model;
        }

        public override DatabaseModel GetCopy()
        {
            return new Team()
            {
                ID = ID,
                Name = Name,
                City = City,
                FoundationDate = FoundationDate,
                StarAmount = StarAmount,
                CoupeAmount= CoupeAmount,
                Info = Info
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
                    case Attribute.ID:              attributes.Add(Attribute.ID.GetDBRepresentation(), ID.ToString());                                                              break;
                    case Attribute.Name:            attributes.Add(Attribute.Name.GetDBRepresentation(), Name == null ? NullVal : Quote + Name + Quote);                            break;
                    case Attribute.City:            attributes.Add(Attribute.City.GetDBRepresentation(), City == null ? NullVal : Quote + City + Quote);                            break;
                    case Attribute.FoundationDate:  attributes.Add(Attribute.FoundationDate.GetDBRepresentation(), FoundationDate == null ? NullVal : FoundationDate.ToString());   break;
                    case Attribute.StarAmount:      attributes.Add(Attribute.StarAmount.GetDBRepresentation(), StarAmount.ToString());                                              break;
                    case Attribute.CoupeAmount:     attributes.Add(Attribute.CoupeAmount.GetDBRepresentation(), CoupeAmount.ToString());                                            break;
                    case Attribute.Info:            attributes.Add(Attribute.Info.GetDBRepresentation(), Info == null ? NullVal : Quote + Info + Quote);                            break;
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
                    case TeamAttributeExtensions.AttrID:                ID = Convert.ToInt32(attribute.Value);                                          break;
                    case TeamAttributeExtensions.AttrName:              Name = attribute.Value.Equals(NullVal) ? null : attribute.Value.Trim(Quote);    break;
                    case TeamAttributeExtensions.AttrCity:              City = attribute.Value.Equals(NullVal) ? null : attribute.Value.Trim(Quote);    break;
                    case TeamAttributeExtensions.AttrFoundationDate:    FoundationDate = Convert.ToDateTime(attribute.Value).Date;                      break;
                    case TeamAttributeExtensions.AttrStarAmount:        StarAmount = Convert.ToInt32(attribute.Value);                                  break;
                    case TeamAttributeExtensions.AttrCoupeAmount:       CoupeAmount = Convert.ToInt32(attribute.Value);                                 break;
                    case TeamAttributeExtensions.AttrInfo:              Info = attribute.Value.Equals(NullVal) ? null : attribute.Value.Trim(Quote);    break;
                    default: break;
                }
            }
        }
    }

    public static class TeamAttributeExtensions
    {
        public const string AttrID              = "id";
        public const string AttrName            = "name";
        public const string AttrCity            = "city";
        public const string AttrFoundationDate  = "found_date";
        public const string AttrStarAmount      = "coupe_amount";
        public const string AttrCoupeAmount     = "star_amount";
        public const string AttrInfo            = "info";
        public const string AttrNone            = "";

        public static string GetDBRepresentation(this Team.Attribute attribute)
        {
            switch (attribute)
            {
                case Team.Attribute.ID:                 return AttrID;
                case Team.Attribute.Name:               return AttrName;
                case Team.Attribute.City:               return AttrCity;
                case Team.Attribute.FoundationDate:     return AttrFoundationDate;
                case Team.Attribute.StarAmount:         return AttrStarAmount;
                case Team.Attribute.CoupeAmount:        return AttrCoupeAmount;
                case Team.Attribute.Info:               return AttrInfo;
                default:                                return AttrNone;
            }
        }
    }
}