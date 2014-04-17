using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SqlTools
{

    class SqlTable
    {
        private string _Table;

        public SqlTable(string name)
        {
            _Table = name;
        }

        public new string ToString()
        {
            return _Table;
        }
    }

    abstract class Clause
    {
        public Dictionary<string, string> Values { get { return _Values; } }

        private Dictionary<string, string> _Values;

        protected string _Prefix;

        public Clause(Dictionary<string, string> values)
        {
            _Values = values;
        }

        public new string ToString()
        {
            string values = string.Join(", ", _Values.Select(v => v.Key + "=" + v.Value).ToArray());

            return string.Format("{0} {1}", _Prefix, values);
        }
    }

    class WhereClause : Clause
    {
        public WhereClause(Dictionary<string, string> values)
            : base(values)
        {
            _Prefix = "WHERE";
        }
    }

    class SetClause : Clause
    {
        public SetClause(Dictionary<string, string> values)
            : base(values)
        {
            _Prefix = "SET";
        }
    }

    class InsertStatement
    {
        private string _Sql;

        private SqlTable _Table;

        private Dictionary<string, string> _Values;

        private Regex _Re = new Regex(@"insert\s+([^\s]+)\s+\((.+)\)\s+values\s+\((.+)\)", RegexOptions.IgnoreCase);

        public SqlTable Table { get { return _Table; } }

        public InsertStatement(string sql)
        {
            _Sql = sql;

            Match m = _Re.Match(sql);

            _Table = new SqlTable(m.Groups[1].Value.Trim());

            _Values = CdlsToDict(m.Groups[2].Value.Trim(), m.Groups[3].Value.Trim());
            
        }

        private Dictionary<string, string> CdlsToDict(string cols, string vals)
        {
            string[] columnArr = cols.Split(',');

            string[] valArr = vals.Split(',');

            if (columnArr.Length != valArr.Length)
            {
                throw new ApplicationException("The insert statement had inequal numbers of columns and values");
            }

            Dictionary<string, string> retVal = new Dictionary<string, string>();

            for (int i = 0; i < columnArr.Length; i++)
            {
                retVal.Add(columnArr[i].Trim(), valArr[i].Trim());
            }

            return retVal;
        }

        internal Dictionary<string, string> GetValues()
        {
            return _Values;
        }
    }

    class UpdateSqlStatement
    {
        private SqlTable _Table;
        private Clause _Where;
        private Clause _Set;

        public UpdateSqlStatement(SqlTable table, Clause where, Clause set)
        {
            _Table = table;
            _Where = where;
            _Set = set;
        }

        public new string ToString()
        {
            return string.Format("UPDATE {0} {1} {2}", _Table.ToString(), _Set.ToString(), _Where.ToString());
        }
    }
    

    class InsertToUpdateSqlConverter
    {
        private Func<string, Clause> _Where;
        private Func<string, Clause> _Update;


        public InsertToUpdateSqlConverter(Func<string, Clause> where, Func<string, Clause> update)
        {
            _Where = where;
            _Update = update;
        }

        public InsertToUpdateSqlConverter(Func<string, Clause> where)
        {
            _Where = where;
        }

        public string Convert(string inputStatement)
        {
            InsertStatement insertStatement = new InsertStatement(inputStatement);

            Clause whereClause = _Where(inputStatement);

            Clause updateClause = Update(inputStatement, insertStatement, whereClause);

            var statement = new UpdateSqlStatement(insertStatement.Table, whereClause, updateClause);

            return statement.ToString();
        }

        private Clause Update(string insertSql, InsertStatement insert, Clause where)
        {
            if (_Update == null)
            {
                Dictionary<string, string> allValues = insert.GetValues();
                Dictionary<string, string> keyValues = where.Values;

                Dictionary<string, string> remainingValues = allValues.Where(v => !keyValues.Contains(v)).ToDictionary(v => v.Key, v => v.Value);

                return new SetClause(remainingValues);
            }
            else
            {
                return _Update(insertSql);
            }
        }

        public static Func<string, Clause> Where(params string[] filters)
        {
            return new Func<string, Clause>(delegate(string insert) 
                {
                    return MakeClause(insert, filters, new Func<Dictionary<string, string>, Clause>(delegate(Dictionary<string, string> columns)
                        {
                            return new WhereClause(columns);
                        }));
                });
        }

        public static Func<string, Clause> Update(params string[] fields)
        {
            return new Func<string, Clause>(delegate(string insert) 
                {
                    return MakeClause(insert, fields, new Func<Dictionary<string, string>, Clause>(delegate(Dictionary<string, string> columns)
                        {
                            return new SetClause(columns);
                        }));
                });
        }

        private static Clause MakeClause(string insertSql, string[] filter, Func<Dictionary<string, string>, Clause> clauseFactory)
        {
            InsertStatement insertStatement = new InsertStatement(insertSql);

            Dictionary<string, string> values = insertStatement.GetValues();

            Dictionary<string, string> requiredValues = values.Where(v => filter.Contains(v.Key)).ToDictionary(v => v.Key, v => v.Value);

            return clauseFactory(requiredValues);
        }
    }
}
