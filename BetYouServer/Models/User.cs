using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetYouServer.Models
{
    public class User : DatabaseModel, IServerModel
    {
        public const ServerModel Model = ServerModel.User;
        private const string DBTableName = "user";

        public enum Attribute { Account, BetPermission, SocialPermission, Balance, VirtualBalance, Rating, Member }
        public enum Membership { Bronze, Silver, Gold }

        public Account Account;
        public bool BetPermission;
        public bool SocialPermission;
        public float Balance;
        public float VirtualBalance;
        public int Rating;
        public Membership Member;

        //DB Purpose - Clear before use! Add only next DBQuery related attributes 
        public List<Attribute> Attributes = new List<Attribute>();

        public ServerModel GetServerModel()
        {
            return Model;
        }

        public override DatabaseModel GetCopy()
        {
            return new User()
            {
                Account = Account,
                BetPermission = BetPermission,
                SocialPermission = SocialPermission,
                Balance = Balance,
                VirtualBalance = VirtualBalance,
                Rating = Rating,
                Member = Member
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
                    case Attribute.Account:             attributes.Add(Attribute.Account.GetDBRepresentation(), Account == null ? NullVal : Account.ID.ToString());     break;
                    case Attribute.BetPermission:       attributes.Add(Attribute.BetPermission.GetDBRepresentation(), BetPermission.ToString());                        break;
                    case Attribute.SocialPermission:    attributes.Add(Attribute.SocialPermission.GetDBRepresentation(), SocialPermission.ToString());                  break;
                    case Attribute.Balance:             attributes.Add(Attribute.Balance.GetDBRepresentation(), Balance.ToString());                                    break;
                    case Attribute.VirtualBalance:      attributes.Add(Attribute.VirtualBalance.GetDBRepresentation(), VirtualBalance.ToString());                      break;
                    case Attribute.Rating:              attributes.Add(Attribute.Rating.GetDBRepresentation(), Rating.ToString());                                      break;
                    case Attribute.Member:              attributes.Add(Attribute.Member.GetDBRepresentation(), Quote + Member.ToString() + Quote);                      break;
                    default:                                                                                                                                            break;
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
                    case UserAttributeExtensions.AttrAccount:           Account             = new Account() { ID = Convert.ToInt32(attribute.Value) };  break;
                    case UserAttributeExtensions.AttrBetPermission:     BetPermission       = Convert.ToBoolean(attribute.Value);                       break;
                    case UserAttributeExtensions.AttrSocialPermission:  SocialPermission    = Convert.ToBoolean(attribute.Value);                       break;
                    case UserAttributeExtensions.AttrBalance:           Balance             = Convert.ToSingle(attribute.Value);                        break;
                    case UserAttributeExtensions.AttrVirtualBalance:    VirtualBalance      = Convert.ToSingle(attribute.Value);                        break;
                    case UserAttributeExtensions.AttrRating:            Rating              = Convert.ToInt32(attribute.Value);                         break;
                    case UserAttributeExtensions.AttrMember:            Member              = Enum.Parse<Membership>(attribute.Value.Trim(Quote));      break;
                    default:                                                                                                                            break;
                }
            }
        }
    }

    public static class UserAttributeExtensions
    {
        public const string AttrAccount            = "account_id";
        public const string AttrBetPermission      = "bet_permission";
        public const string AttrSocialPermission   = "social_permission";
        public const string AttrBalance            = "balance";
        public const string AttrVirtualBalance     = "virtual_balance";
        public const string AttrRating             = "rating";
        public const string AttrMember             = "membership";
        public const string AttrNone               = "";

        public static string GetDBRepresentation(this User.Attribute attribute)
        {
            switch (attribute)
            {
                case User.Attribute.Account:            return AttrAccount;
                case User.Attribute.BetPermission:      return AttrBetPermission;
                case User.Attribute.SocialPermission:   return AttrSocialPermission;
                case User.Attribute.Balance:            return AttrBalance;
                case User.Attribute.VirtualBalance:     return AttrVirtualBalance;
                case User.Attribute.Rating:             return AttrRating;
                case User.Attribute.Member:             return AttrMember;
                default:                                return AttrNone;
            }
        }
    }

}
