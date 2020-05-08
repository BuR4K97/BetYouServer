using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetYouServer.Controllers
{
    public class DatabaseQuery
    {
        public enum Statement { Insert, Update, Delete, Select }
        public enum Clause { From, Where, GroupBy, Having, With }
        public enum Join { Natural, ROuter, LOuter, FOuter, Cartesian }
        public enum Operation { Rename, Union, Intersect, Except }
        public enum Function { Avg, Min, Max, Sum, Count }
        public enum Condition { Not, And, Or, Equal, Is, Like, Less, LessEq, Greater, GreaterEq, Between, All, Some, In, Exists, Unique }
        public enum Spec { All, Distinct }
        public enum Helper { Values, Set, Space, Comma, Quote, LParanthesis, RParanthesis, As, Semicolon }
        public enum SpecialVal { Null }

        public string GetSQLRepresentation()
        {
            return null;
        }

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
                case DatabaseQuery.Helper.Quote:            return "'";
                case DatabaseQuery.Helper.LParanthesis:     return "(";
                case DatabaseQuery.Helper.RParanthesis:     return ")";
                case DatabaseQuery.Helper.As:               return "AS";
                case DatabaseQuery.Helper.Semicolon:        return ";";
                default:                                    return "";
            }
        }
    }

    public static class DBQSpecialValExtensions
    {
        public static string GetSQLRepresentation(this DatabaseQuery.SpecialVal specialVal)
        {
            switch (specialVal)
            {
                case DatabaseQuery.SpecialVal.Null:     return "NULL";
                default:                                return "";
            }
        }
    }

}
