using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using BetYouServer.Configurations;
using BetYouServer.Models;

namespace BetYouServer.Controllers
{
    public class DBConnectionController
    {
        private const int MAX_CONNECTION = 20;
        private static readonly DatabaseConnectionSpec _dbConnSpec = new DatabaseConnectionSpec()
        {
            Server = DatabaseConfiguration.Server,
            Database = DatabaseConfiguration.Database,
            UserID = DatabaseConfiguration.UserID,
            Password = DatabaseConfiguration.Password
        };

        private readonly List<DatabaseConnection> _availableConns;
        private readonly List<DatabaseConnection> _unavailableConns;
        private readonly Semaphore _availableConnResource;

        public DBConnectionController()
        {
            _availableConns = new List<DatabaseConnection>(MAX_CONNECTION);
            _unavailableConns = new List<DatabaseConnection>(MAX_CONNECTION);
            _availableConnResource = new Semaphore(MAX_CONNECTION, MAX_CONNECTION);
            for (int i = 0; i < MAX_CONNECTION; i++)
            {
                _availableConns.Add(new DatabaseConnection(_dbConnSpec));
                _availableConns.Last().Connect();
            }
        }

        public DatabaseConnection RetrieveDBConnection()
        {
            DatabaseConnection conn;
            _availableConnResource.WaitOne();
            lock (_availableConns)
            {
                conn = _availableConns.Last();
                _availableConns.Remove(conn);
            }
            lock (_unavailableConns)
            {
                _unavailableConns.Add(conn);
            }
            return conn;
        }

        public void ReleaseDBConnection(DatabaseConnection conn)
        {
            lock (_unavailableConns)
            {
                if (!_unavailableConns.Remove(conn)) return;
            }
            lock (_availableConns)
            {
                _availableConns.Add(conn);
            }
            _availableConnResource.Release();
        }
    }
}
