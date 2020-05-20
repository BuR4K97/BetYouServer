using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetYouServer.Models
{
    public class Admin : Actor
    {
        public const ServerModel Model = ServerModel.Admin;
        private const string DBTableName = "admin";

        public enum Attribute { Account, Nickname }

        public string Nickname { get; set; }

        //DB Purpose - Clear before use! Add only next DBQuery related attributes 
        public readonly List<Attribute> Attributes = new List<Attribute>();

        public override ServerModel GetServerModel()
        {
            return Model;
        }

        public override DatabaseModel GetCopy()
        {
            return new Admin()
            {
                Account = Account,
                Nickname = Nickname
            };
        }

         public override string GetTableName()
        {
            return DBTableName;
        }

        public override Dictionary<string, string> GetAttributes()
        {
            Dictionary<string, string> attributes = new Dictionary<string, string>();
            foreach (Attribute attribute in Attributes)
            {
                switch (attribute)
                {
                    case Attribute.Account:     attributes.Add(Attribute.Account.GetDBRepresentation(),     Account == null ? NullVal : Account.ID.ToString());     break;
                    case Attribute.Nickname:    attributes.Add(Attribute.Nickname.GetDBRepresentation(),    Nickname == null ? NullVal : Quote + Nickname + Quote); break;
                    default:                                                                                                                                        break;
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
                    case AdminAttributeExtensions.AttrAccount:  Account     = new Account() { ID = Convert.ToInt32(attribute.Value) };  break;
                    case AdminAttributeExtensions.AttrNickname: Nickname    = attribute.Value.Trim(Quote);                              break;
                    default:                                                                                                            break;
                }
            }
        }
    }

    public static class AdminAttributeExtensions
    {
        public const string AttrAccount            = "account_id";
        public const string AttrNickname           = "nickname";
        public const string AttrNone               = "";

        public static string GetDBRepresentation(this Admin.Attribute attribute)
        {
            switch (attribute)
            {
                case Admin.Attribute.Account:           return AttrAccount;
                case Admin.Attribute.Nickname:          return AttrNickname;
                default:                                return AttrNone;
            }
        }
    }
}
