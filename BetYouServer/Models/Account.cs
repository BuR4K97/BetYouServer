using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetYouServer.Models
{
    public class Account : DatabaseModel, IServerModel
    {
        public const ServerModel Model = ServerModel.Account;
        private const string DBTableName = "account";

        public enum Attribute { ID, Username, Password, Forename, Surname, Email, PicLink }

        public int ID;
        public string Username;
        public string Password;
        public string Forename;
        public string Surname;
        public string Email;
        public string PicLink;

        //DB Purpose - Clear before use! Add only next DBQuery related attributes 
        public List<Attribute> Attributes = new List<Attribute>();

        public ServerModel GetServerModel()
        {
            return Model;
        }

        public override DatabaseModel GetCopy()
        {
            return new Account()
            {
                ID = ID,
                Username = Username,
                Password = Password,
                Forename = Forename,
                Surname = Surname,
                Email = Email,
                PicLink = PicLink
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
                    case Attribute.ID:          attributes.Add(Attribute.ID.GetDBRepresentation(),          ID.ToString());                                             break;
                    case Attribute.Username:    attributes.Add(Attribute.Username.GetDBRepresentation(),    Username    == null ? NullVal : Quote + Username + Quote);  break;
                    case Attribute.Password:    attributes.Add(Attribute.Password.GetDBRepresentation(),    Password    == null ? NullVal : Quote + Password + Quote);  break;
                    case Attribute.Forename:    attributes.Add(Attribute.Forename.GetDBRepresentation(),    Forename    == null ? NullVal : Quote + Forename + Quote);  break;
                    case Attribute.Surname:     attributes.Add(Attribute.Surname.GetDBRepresentation(),     Surname     == null ? NullVal : Quote + Surname + Quote);   break;
                    case Attribute.Email:       attributes.Add(Attribute.Email.GetDBRepresentation(),       Email       == null ? NullVal : Quote + Email + Quote);     break;
                    case Attribute.PicLink:     attributes.Add(Attribute.PicLink.GetDBRepresentation(),     PicLink     == null ? NullVal : Quote + PicLink + Quote);   break;
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
                    case AccountAttributeExtensions.AttrID:         ID          = Convert.ToInt32(attribute.Value);                                             break;
                    case AccountAttributeExtensions.AttrUsername:   Username    = attribute.Value.Trim(Quote);                                                  break;
                    case AccountAttributeExtensions.AttrPassword:   Password    = attribute.Value.Trim(Quote);                                                  break;
                    case AccountAttributeExtensions.AttrForename:   Forename    = attribute.Value.Trim(Quote);                                                  break;
                    case AccountAttributeExtensions.AttrSurname:    Surname     = attribute.Value.Trim(Quote);                                                  break;
                    case AccountAttributeExtensions.AttrEmail:      Email       = attribute.Value.Trim(Quote);                                                  break;
                    case AccountAttributeExtensions.AttrPicLink:    PicLink     = attribute.Value.Equals(NullVal) ? null : attribute.Value.Trim(Quote);         break;
                    default:                                                                                                                                    break;
                }
            }
        }
    }

    public static class AccountAttributeExtensions
    {
        public const string AttrID          = "id";
        public const string AttrUsername    = "username";
        public const string AttrPassword    = "password";
        public const string AttrForename    = "forename";
        public const string AttrSurname     = "surname";
        public const string AttrEmail       = "email";
        public const string AttrPicLink     = "pic_link";
        public const string AttrNone        = "";

        public static string GetDBRepresentation(this Account.Attribute attribute)
        {
            switch (attribute)
            {
                case Account.Attribute.ID:          return AttrID;
                case Account.Attribute.Username:    return AttrUsername;
                case Account.Attribute.Password:    return AttrPassword;
                case Account.Attribute.Forename:    return AttrForename;
                case Account.Attribute.Surname:     return AttrSurname;
                case Account.Attribute.Email:       return AttrEmail;
                case Account.Attribute.PicLink:     return AttrPicLink;
                default:                            return AttrNone;
            }
        }
    }
}