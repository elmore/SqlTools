using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.ObjectBuilder2;

namespace SqlElements
{
    public class SqlSelect : SqlStatement
    {
        protected readonly string _table;
        private string _fields;
        private List<SqlOrder> _orders = new List<SqlOrder>();
        private int _top = -1;

        public SqlSelect(string table)
        {
            _table = table;
        }

        public SqlSelect Top(int count)
        {
            _top = count;
            return this;
        }

        public override string ToString()
        {
            string conditions = _values
                .Select(kvp => string.Format("{0} = {1}", kvp.Key, kvp.Value))
                .JoinStrings(" and ");

            if (!string.IsNullOrWhiteSpace(conditions))
            {
                conditions = " where " + conditions;
            }

            string order = _orders.JoinStrings(" , ");

            if (!string.IsNullOrWhiteSpace(order))
            {
                order = " order by " + order;
            }

            string top = string.Empty;

            if (_top > -1)
            {
                top = string.Format(" top ({0}) ", _top);
            }

            return string.Format("select {0} * from {1} {2} {3}", top, _table, conditions, order);
        }

        public void AddOrder(SqlOrder sqlOrder)
        {
            _orders.Add(sqlOrder);
        }
    }
}