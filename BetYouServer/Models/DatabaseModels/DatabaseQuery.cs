using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace BetYouServer.Models
{
    public class DatabaseQuery //: DatabaseModel
    {
        public enum Statement   { Insert, Update, Delete, Select, None }
        public enum Clause      { From, Where, GroupBy, Having, With }
        public enum Join        { Natural, ROuter, LOuter, FOuter, Cartesian, None }
        public enum Operation   { Rename, Union, Intersect, Except }
        public enum Function    { Avg, Min, Max, Sum, Count }
        public enum Condition   { Not, And, Or, Equal, Is, Like, Less, LessEq, Greater, GreaterEq, Between, All, Some, In, Exists, Unique }
        public enum Spec        { All, Distinct }
        public enum Helper      { Values, Set, Space, Comma, Dot, Equal, Add, Subtract, Multiply, Divide, LParanthesis, RParanthesis, As, Semicolon }

        public static DatabaseQuery ConstructInsertStatement(DatabaseModel insertModel) 
        {
            return new DatabaseQuery()
            {
                _statement = Statement.Insert,
                _insertModel = insertModel
            };
        }

        public static DatabaseQuery ConstructUpdateStatement(DatabaseModel setModel, List<Helper> setters, DatabaseModel conditionModel, List<Condition> conditioners
            , List<DatabaseQuery> conditionExtensions = null)
        {
            return new DatabaseQuery()
            {
                _statement = Statement.Update,
                _setModel = setModel,
                _setters = setters,
                _conditionModel = conditionModel,
                _conditioners = conditioners,
                _conditionExtensions = conditionExtensions
            };
        }

        public static DatabaseQuery ConstructSelectStatement(DatabaseModel selectModel, DatabaseModel conditionModel, List<Condition> conditioners
            , List<DatabaseQuery> conditionExtensions = null)
        {
            return new DatabaseQuery()
            {
                _statement = Statement.Select,
                _selectModel = selectModel,
                _conditionModel = conditionModel,
                _conditioners = conditioners,
                _conditionExtensions = conditionExtensions,
                Results = new List<DatabaseModel>()
            };
        }

        public static DatabaseQuery ConstructDeleteStatement(DatabaseModel deleteModel, DatabaseModel conditionModel, List<Condition> conditioners
            , List<DatabaseQuery> conditionExtensions = null)
        {
            return new DatabaseQuery()
            {
                _statement = Statement.Delete,
                _deleteModel = deleteModel,
                _conditionModel = conditionModel,
                _conditioners = conditioners,
                _conditionExtensions = conditionExtensions
            };
        }

        private Statement _statement;
        private DatabaseModel _insertModel;
        private DatabaseModel _setModel;
        private DatabaseModel _selectModel;
        private DatabaseModel _deleteModel;
        private List<Helper> _setters;
        private DatabaseModel _conditionModel;
        private List<Condition> _conditioners;
        private List<DatabaseQuery> _conditionExtensions;

        public List<DatabaseModel> Results;

        public void FeedSQLResult(Dictionary<string, string> attributes)
        {
            Results.Add(_selectModel.GetCopy());
            Results.Last().SetAttributes(attributes);
        }

        public string GetSQLRepresentation()
        {
            Dictionary<string, string> attributes;
            List<string> keys;

            string query = "";
            switch (_statement)
            {
                case Statement.Insert:

                    attributes = _insertModel.GetAttributes();
                    query += Statement.Insert.GetSQLRepresentation() + Helper.Space.GetSQLRepresentation() + _insertModel.GetTableName();
                    query += Helper.LParanthesis.GetSQLRepresentation();
                    foreach (string key in attributes.Keys)
                    {
                        if (!ReferenceEquals(key, attributes.Keys.First())) query += Helper.Comma.GetSQLRepresentation();
                        query += key;
                    }
                    query += Helper.RParanthesis.GetSQLRepresentation();
                    query += Helper.Space.GetSQLRepresentation() + Helper.Values.GetSQLRepresentation();
                    query += Helper.LParanthesis.GetSQLRepresentation();
                    foreach (string val in attributes.Values)
                    {
                        if (!ReferenceEquals(val, attributes.Values.First())) query += Helper.Comma.GetSQLRepresentation();
                        query += val;
                    }
                    query += Helper.RParanthesis.GetSQLRepresentation();
                    query += Helper.Semicolon.GetSQLRepresentation();
                    break;

                case Statement.Update:

                    attributes = _setModel.GetAttributes();
                    query += Statement.Update.GetSQLRepresentation() + Helper.Space.GetSQLRepresentation() + _setModel.GetTableName() 
                        + Helper.Space.GetSQLRepresentation() + Helper.Set.GetSQLRepresentation() + Helper.Space.GetSQLRepresentation();
                    for(int i = 0; i < attributes.Count; i++)
                    {
                        if (i != 0) query += Helper.Comma.GetSQLRepresentation();
                        query += attributes.ElementAt(i).Key;
                        switch (_setters.ElementAt(i))
                        {
                            case Helper.Equal:      query += Helper.Equal.GetSQLRepresentation();                                          break;
                            case Helper.Add:        query += Helper.Add.GetSQLRepresentation()      + Helper.Equal.GetSQLRepresentation(); break;
                            case Helper.Subtract:   query += Helper.Subtract.GetSQLRepresentation() + Helper.Equal.GetSQLRepresentation(); break;
                            case Helper.Multiply:   query += Helper.Multiply.GetSQLRepresentation() + Helper.Equal.GetSQLRepresentation(); break;
                            case Helper.Divide:     query += Helper.Divide.GetSQLRepresentation()   + Helper.Equal.GetSQLRepresentation(); break;
                            default:                throw new InvalidDBQSetterException();
                        }
                        query += attributes.ElementAt(i).Value;
                    }
                    if (GetConditionStatus()) query += Helper.Space.GetSQLRepresentation() + GetConditionSQLRepresentation();
                    query += Helper.Semicolon.GetSQLRepresentation();
                    break;

                case Statement.Select:

                    keys = _selectModel.GetAttributes().Keys.ToList();
                    query += Statement.Select.GetSQLRepresentation() + Helper.Space.GetSQLRepresentation();
                    foreach (string key in keys)
                    {
                        if (!ReferenceEquals(key, keys.First())) query += Helper.Comma.GetSQLRepresentation();
                        query += key;
                    }
                    query += Helper.Space.GetSQLRepresentation() + Clause.From.GetSQLRepresentation() + Helper.Space.GetSQLRepresentation() + _selectModel.GetTableName();
                    if (GetConditionStatus()) query += Helper.Space.GetSQLRepresentation() + GetConditionSQLRepresentation();
                    query += Helper.Semicolon.GetSQLRepresentation();
                    break;

                case Statement.Delete:

                    query += Statement.Delete.GetSQLRepresentation() + Helper.Space.GetSQLRepresentation() + Clause.From.GetSQLRepresentation()
                        + Helper.Space.GetSQLRepresentation() + _deleteModel.GetTableName();
                    if(GetConditionStatus()) query += Helper.Space.GetSQLRepresentation() + GetConditionSQLRepresentation();
                    query += Helper.Semicolon.GetSQLRepresentation();
                    break;

                case Statement.None:

                default: break;

            }
            return query;
        }

        private bool GetConditionStatus()
        {
            return _conditionModel.GetAttributes().Count > 0;
        }

        private string GetConditionSQLRepresentation()
        {
            Dictionary<string, string> attributes;
            int extensionCounter;
            string query = "";

            attributes = _conditionModel.GetAttributes();
            if (attributes.Count > 0) query += Clause.Where.GetSQLRepresentation() + Helper.Space.GetSQLRepresentation();
            extensionCounter = 0;
            for (int i = 0; i < attributes.Count; i++)
            {
                if (i != 0) query += Helper.Space.GetSQLRepresentation() + Condition.And.GetSQLRepresentation() + Helper.Space.GetSQLRepresentation();
                query += attributes.ElementAt(i).Key + Helper.Space.GetSQLRepresentation();
                switch (_conditioners.ElementAt(i))
                {
                    case Condition.Equal:       query += Condition.Equal.GetSQLRepresentation()     + Helper.Space.GetSQLRepresentation() + attributes.ElementAt(i).Value;  break;
                    case Condition.Is:          query += Condition.Is.GetSQLRepresentation()        + Helper.Space.GetSQLRepresentation() + attributes.ElementAt(i).Value;  break;
                    case Condition.Less:        query += Condition.Less.GetSQLRepresentation()      + Helper.Space.GetSQLRepresentation() + attributes.ElementAt(i).Value;  break;
                    case Condition.LessEq:      query += Condition.LessEq.GetSQLRepresentation()    + Helper.Space.GetSQLRepresentation() + attributes.ElementAt(i).Value;  break;
                    case Condition.Greater:     query += Condition.Greater.GetSQLRepresentation()   + Helper.Space.GetSQLRepresentation() + attributes.ElementAt(i).Value;  break;
                    case Condition.GreaterEq:   query += Condition.GreaterEq.GetSQLRepresentation() + Helper.Space.GetSQLRepresentation() + attributes.ElementAt(i).Value;  break;
                    case Condition.Between:     query += _conditionExtensions.ElementAt(extensionCounter++).GetSQLRepresentation();                                         break;
                    case Condition.All:         query += _conditionExtensions.ElementAt(extensionCounter++).GetSQLRepresentation();                                         break;
                    case Condition.Some:        query += _conditionExtensions.ElementAt(extensionCounter++).GetSQLRepresentation();                                         break;
                    case Condition.In:          query += _conditionExtensions.ElementAt(extensionCounter++).GetSQLRepresentation();                                         break;
                    case Condition.Exists:      query += _conditionExtensions.ElementAt(extensionCounter++).GetSQLRepresentation();                                         break;
                    case Condition.Unique:      query += _conditionExtensions.ElementAt(extensionCounter++).GetSQLRepresentation();                                         break;
                    default:                    throw new InvalidDBQConditionerException();
                }
            }
            return query;
        }

    }

    public class InvalidDBQSetterException : Exception
    {
        private const string Message = "";

        public InvalidDBQSetterException() : base(Message) {}

    }

    public class InvalidDBQConditionerException : Exception
    {
        private const string Message = "";

        public InvalidDBQConditionerException() : base(Message) {}

    }

    public static class DBQStatementExtensions
    {
        public static string GetSQLRepresentation(this DatabaseQuery.Statement statement)
        {
            switch (statement)
            {
                case DatabaseQuery.Statement.Insert:    return "INSERT";
                case DatabaseQuery.Statement.Update:    return "UPDATE";
                case DatabaseQuery.Statement.Delete:    return "DELETE";
                case DatabaseQuery.Statement.Select:    return "SELECT";
                default:                                return "";
            }
        }
    }

    public static class DBQClauseExtensions
    {
        public static string GetSQLRepresentation(this DatabaseQuery.Clause clause)
        {
            switch (clause)
            {
                case DatabaseQuery.Clause.From:     return "FROM";
                case DatabaseQuery.Clause.Where:    return "WHERE";
                case DatabaseQuery.Clause.GroupBy:  return "GROUP BY";
                case DatabaseQuery.Clause.Having:   return "HAVING";
                case DatabaseQuery.Clause.With:     return "WITH";
                default:                            return "";
            }
        }
    }

    public static class DBQJoinExtensions
    {
        public static string GetSQLRepresentation(this DatabaseQuery.Join join)
        {
            switch (join)
            {
                case DatabaseQuery.Join.Natural:    return "NATURAL JOIN";
                case DatabaseQuery.Join.LOuter:     return "LEFT OUTER JOIN";
                case DatabaseQuery.Join.ROuter:     return "RIGHT OUTER JOIN";
                case DatabaseQuery.Join.FOuter:     return "FULL OUTER JOIN";
                case DatabaseQuery.Join.Cartesian:  return ",";
                default:                            return "";
            }
        }
    }

    public static class DBQOperationExtensions
    {
        public static string GetSQLRepresentation(this DatabaseQuery.Operation operation)
        {
            switch (operation)
            {
                case DatabaseQuery.Operation.Rename:        return "AS";
                case DatabaseQuery.Operation.Union:         return "UNION";
                case DatabaseQuery.Operation.Intersect:     return "INTERSECT";
                case DatabaseQuery.Operation.Except:        return "EXCEPT";
                default:                                    return "";
            }
        }
    }

    public static class DBQFunctionExtensions
    {
        public static string GetSQLRepresentation(this DatabaseQuery.Function function)
        {
            switch (function)
            {
                case DatabaseQuery.Function.Avg:    return "AVG";
                case DatabaseQuery.Function.Min:    return "MIN";
                case DatabaseQuery.Function.Max:    return "MAX";
                case DatabaseQuery.Function.Sum:    return "SUM";
                case DatabaseQuery.Function.Count:  return "COUNT";
                default:                            return "";
            }
        }
    }

    public static class DBQConditionExtensions
    {
        public static string GetSQLRepresentation(this DatabaseQuery.Condition condition)
        {
            switch (condition)
            {
                case DatabaseQuery.Condition.Not:           return "NOT";
                case DatabaseQuery.Condition.And:           return "AND";
                case DatabaseQuery.Condition.Or:            return "OR";
                case DatabaseQuery.Condition.Equal:         return "=";
                case DatabaseQuery.Condition.Is:            return "IS";
                case DatabaseQuery.Condition.Like:          return "LIKE";
                case DatabaseQuery.Condition.Less:          return "<";
                case DatabaseQuery.Condition.LessEq:        return "<=";
                case DatabaseQuery.Condition.Greater:       return ">";
                case DatabaseQuery.Condition.GreaterEq:     return ">=";
                case DatabaseQuery.Condition.Between:       return "BETWEEN";
                case DatabaseQuery.Condition.All:           return "ALL";
                case DatabaseQuery.Condition.Some:          return "SOME";
                case DatabaseQuery.Condition.In:            return "IN";
                case DatabaseQuery.Condition.Exists:        return "EXISTS";
                case DatabaseQuery.Condition.Unique:        return "UNIQUE";
                default:                                    return "";
            }
        }
    }

    public static class DBQSpecExtensions
    {
        public static string GetSQLRepresentation(this DatabaseQuery.Spec spec)
        {
            switch (spec)
            {
                case DatabaseQuery.Spec.All:        return "*";
                case DatabaseQuery.Spec.Distinct:   return "DISTINCT";
                default:                            return "";
            }
        }
    }

    public static class DBQHelperExtensions
    {
        public static string GetSQLRepresentation(this DatabaseQuery.Helper helper)
        {
            switch (helper)
            {
                case DatabaseQuery.Helper.Values:           return "VALUES";
                case DatabaseQuery.Helper.Set:              return "SET";
                case DatabaseQuery.Helper.Space:            return " ";
                case DatabaseQuery.Helper.Comma:            return ",";
                case DatabaseQuery.Helper.Dot:              return ".";
                case DatabaseQuery.Helper.Equal:              return "=";
                case DatabaseQuery.Helper.LParanthesis:     return "(";
                case DatabaseQuery.Helper.RParanthesis:     return ")";
                case DatabaseQuery.Helper.As:               return "AS";
                case DatabaseQuery.Helper.Semicolon:        return ";";
                default:                                    return "";
            }
        }
    }
}
