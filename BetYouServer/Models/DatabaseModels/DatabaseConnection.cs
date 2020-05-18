using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.IO;

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
                Console.Error.WriteLine(exception.Message);
                Console.Error.WriteLine(exception.StackTrace);
            }
            catch (MySqlException exception)
            {
                Console.Error.WriteLine(exception.Message);
                Console.Error.WriteLine(exception.StackTrace);
            }
        }

        public List<Dictionary<string, string>> ExecuteQuery(string query)
        {
            List<Dictionary<string, string>> results = new List<Dictionary<string, string>>();
            MySqlCommand comm = new MySqlCommand(query, _conn);
            MySqlDataReader reader = null;
            try
            {
                reader = comm.ExecuteReader();
            }
            catch (MySqlException exception)
            {
                throw new DBQExecutionFailException(exception.Code, exception.Number, exception.SqlState, exception.Message);
            }

            while (reader.Read())
            {
                results.Add(new Dictionary<string, string>());
                for(int i = 0; i < reader.FieldCount; i++)
                {
                    results.Last().Add(reader.GetName(i), reader.IsDBNull(i) ? DatabaseModel.NullVal : reader.GetString(i));
                }
            }
            comm.Cancel();
            reader.Close();
            return results;
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

    public class DBQExecutionFailException : Exception
    {
        private const string Message = "DatabaseQuery execution failed for a reason! Investigate further details.";

        public readonly uint SQLCode;
        public readonly int SQLType;
        public readonly string SQLState;
        public readonly string InnerExceptMess;

        public DBQExecutionFailException(uint sqlCode, int sqlType, string sqlState, string innerExceptMess) : base(Message)
        {
            this.SQLCode = sqlCode;
            this.SQLType = sqlType;
            this.SQLState = sqlState;
            this.InnerExceptMess = innerExceptMess;
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
