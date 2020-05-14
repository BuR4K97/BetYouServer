using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace BetYouServer.Models
{
    public class DatabaseConnection : IDisposable
    {
        private DatabaseConnectionSpec _connSpec;
        private MySqlConnection _conn;

        public DatabaseConnection(DatabaseConnectionSpec connSpec)
        {
            this._connSpec = connSpec; 
            this._conn = new MySqlConnection(connSpec.GetStringRepresentation());
        }

        public void Connect()
        {
            try
            {
                _conn.Open();
            }
            catch (InvalidOperationException exception)
            {
                Console.WriteLine(exception.Message);
            }
            catch (MySqlException exception)
            {
                Console.WriteLine(exception.Message);
            }
        }

        public void ExecuteQuery(DatabaseQuery query)
        {
            MySqlCommand comm = new MySqlCommand(query.GetSQLRepresentation(), _conn);
            MySqlDataReader reader = comm.ExecuteReader();

            Dictionary<string, string> attributes = new Dictionary<string, string>();
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    attributes.Add(reader.GetName(i), reader.GetString(i));
                }
                query.FeedSQLResult(attributes);
                attributes.Clear();
            }
            reader.Close();
        }

        public void Close()
        {
            _conn.Close();
        }

        public void Dispose()
        {
            Close();
        }

    }

    public class DatabaseConnectionSpec
    {
        public enum Spec { Server, Database, UserID, Password, Timeout, UserVariables, Compression }
        private const string Equal = "=";
        private const string Comma = ";"; 

        public string Server;
        public string Database;
        public string UserID;
        public string Password;
        public int Timeout = 15;
        public bool UserVariables = false;
        public bool Compression = false;

        public string GetStringRepresentation()
        {
            return Spec.Server.GetKeyRepresentation()           + Equal + Server        + Comma 
                    + Spec.Database.GetKeyRepresentation()      + Equal + Database      + Comma
                    + Spec.UserID.GetKeyRepresentation()        + Equal + UserID        + Comma
                    + Spec.Password.GetKeyRepresentation()      + Equal + Password      + Comma
                    + Spec.Timeout.GetKeyRepresentation()       + Equal + Timeout       + Comma
                    + Spec.UserVariables.GetKeyRepresentation() + Equal + UserVariables + Comma
                    + Spec.Compression.GetKeyRepresentation()   + Equal + Compression   + Comma;
        }

    }

    public static class DBConnectionSpecExtensions
    {
        public static string GetKeyRepresentation(this DatabaseConnectionSpec.Spec spec)
        {
            switch (spec)
            {
                case DatabaseConnectionSpec.Spec.Server:                return "Server";
                case DatabaseConnectionSpec.Spec.Database:              return "Database";
                case DatabaseConnectionSpec.Spec.UserID:                return "Uid";
                case DatabaseConnectionSpec.Spec.Password:              return "Pwd";
                case DatabaseConnectionSpec.Spec.Timeout:               return "Connect Timeout";
                case DatabaseConnectionSpec.Spec.UserVariables:         return "AllowUserVariables";
                case DatabaseConnectionSpec.Spec.Compression:           return "UseCompression";
                default:                                                return "";
            }
        }
    }


}
