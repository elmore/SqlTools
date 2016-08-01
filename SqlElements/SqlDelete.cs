using System;
using System.Linq;
using Microsoft.Practices.ObjectBuilder2;

namespace SqlElements
{
    public class SqlDelete : SqlStatement
    {
        protected readonly string _table;
        private string _fields;

        public SqlDelete(string table)
        {
            _table = table;
        }

        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(_table))
            {
                throw new ApplicationException("SqlDelete attempted to delete without a table set");
            }

            string conditions = _values
                .Select(kvp => string.Format("{0} = {1}", kvp.Key, kvp.Value))
                .JoinStrings(" and ");

            if (string.IsNullOrWhiteSpace(conditions))
            {
                throw new ApplicationException("SqlDelete attempted to delete without parameters");
            }

            return string.Format("delete {0} where {1}", _table, conditions);
        }
    }
}
