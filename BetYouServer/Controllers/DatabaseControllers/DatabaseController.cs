using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BetYouServer.Configurations;
using BetYouServer.Models;

namespace BetYouServer.Controllers
{
    public class DatabaseController
    {
        private readonly DBConnectionController DBController = Configuration.GetService<DBConnectionController>();

        public ExecutionResult InsertData(DatabaseModel insert)
        {
            DatabaseQuery query = DatabaseQuery.ConstructInsertStatement(insert);
            ExecutionResult result = new ExecutionResult() { Stat = ExecutionResult.Status.Success, Data = query.Results };

            string execQuery = query.GetSQLRepresentation();
            DatabaseConnection dbConn = DBController.RetrieveDBConnection();
            try
            {
                dbConn.ExecuteQuery(execQuery);
            }
            catch (DBQExecutionFailException exception)
            {
                result.Stat = ExecutionResult.Status.Fail;
                result.FailInfo = exception.InnerExceptMess;
            }
            DBController.ReleaseDBConnection(dbConn);
            return result;
        }

        public ExecutionResult UpdateData(DatabaseModel update, List<DatabaseQuery.Helper> setters, DatabaseModel condition, List<DatabaseQuery.Condition> conditioners
            , List<DatabaseQuery> conditionExts = null)
        {
            DatabaseQuery query = DatabaseQuery.ConstructUpdateStatement(update, setters, condition, conditioners, conditionExts);
            ExecutionResult result = new ExecutionResult() { Stat = ExecutionResult.Status.Success, Data = query.Results };

            string execQuery = query.GetSQLRepresentation();
            DatabaseConnection dbConn = DBController.RetrieveDBConnection();
            try
            {
                dbConn.ExecuteQuery(execQuery);
            }
            catch (DBQExecutionFailException exception)
            {
                result.Stat = ExecutionResult.Status.Fail;
                result.FailInfo = exception.InnerExceptMess;
            }
            DBController.ReleaseDBConnection(dbConn);
            return result;
        }

        public ExecutionResult SelectData(DatabaseModel select, DatabaseModel condition, List<DatabaseQuery.Condition> conditioners, List<DatabaseQuery> conditionExts = null)
        {
            DatabaseQuery query = DatabaseQuery.ConstructSelectStatement(select, condition, conditioners, conditionExts);
            ExecutionResult result = new ExecutionResult() { Stat = ExecutionResult.Status.Success, Data = query.Results };

            List<Dictionary<string, string>> execData = null; string execQuery = query.GetSQLRepresentation();
            DatabaseConnection dbConn = DBController.RetrieveDBConnection();
            try
            {
                execData = dbConn.ExecuteQuery(execQuery);
            }
            catch (DBQExecutionFailException exception)
            {
                result.Stat = ExecutionResult.Status.Fail;
                result.FailInfo = exception.InnerExceptMess;
            }
            DBController.ReleaseDBConnection(dbConn);
            for(int i = 0; i < execData.Count; i++) query.FeedSQLResult(execData.ElementAt(i));
            return result;
        }
    }

    public class ExecutionResult
    {
        public enum Status { Success, Fail }

        public Status Stat;
        public string FailInfo;
        public List<DatabaseModel> Data;
    }

}
