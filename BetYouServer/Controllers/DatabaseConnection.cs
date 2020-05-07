using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace BetYouServer.Controllers
{
    public class DatabaseConnection : IDisposable
    {

        private DatabaseConnectionSpec _connSpec;
        private MySqlConnection _conn;

        public DatabaseConnection(DatabaseConnectionSpec connSpec)
        {
            this._connSpec = connSpec; 
            this._conn = new MySqlConnection
                (
                    DatabaseConnectionSpec.Spec.Server.GetKeyRepresentation()           + connSpec.Server           + ";"
                    + DatabaseConnectionSpec.Spec.Database.GetKeyRepresentation()       + connSpec.Database         + ";"
                    + DatabaseConnectionSpec.Spec.UserID.GetKeyRepresentation()         + connSpec.UserID           + ";"
                    + DatabaseConnectionSpec.Spec.Password.GetKeyRepresentation()       + connSpec.Password         + ";"
                    + DatabaseConnectionSpec.Spec.Timeout.GetKeyRepresentation()        + connSpec.Timeout          + ";"
                    + DatabaseConnectionSpec.Spec.Encyrption.GetKeyRepresentation()     + connSpec.Encyrption       + ";"
                    + DatabaseConnectionSpec.Spec.UserVariables.GetKeyRepresentation()  + connSpec.UserVariables    + ";"
                    + DatabaseConnectionSpec.Spec.Compression.GetKeyRepresentation()    + connSpec.Compression      + ";"
                );
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

        public void ExecuteQuery()
        {
            string query = "SELECT col0,col1 FROM YourTable";
            MySqlCommand comm = new MySqlCommand(query, _conn);
            MySqlDataReader reader = comm.ExecuteReader();
            while (reader.Read())
            {

                int x = reader.FieldCount;
                Console.WriteLine();
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
        public enum Spec { Server, Database, UserID, Password, Timeout, Encyrption, UserVariables, Compression }

        public string Server;
        public string Database;
        public string UserID;
        public string Password;
        public int Timeout = 15;
        public bool Encyrption = false;
        public bool UserVariables = false;
        public bool Compression = false;

    }

    public static class ConnectionSpecExtensions
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
                case DatabaseConnectionSpec.Spec.Encyrption:            return "Encyrpt";
                case DatabaseConnectionSpec.Spec.UserVariables:         return "AllowUserVariables";
                case DatabaseConnectionSpec.Spec.Compression:           return "UseCompression";
                default:                                                return "";
            }
        }
    }


}
