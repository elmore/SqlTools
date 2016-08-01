using System;

namespace SqlElements.SqlTypes
{
    public class SqlDateTime : SqlType
    {
        public SqlDateTime(DateTime val) : base(val) {}

        public override string ToString()
        {
            return string.Format("'{0:yyyy-MM-dd hh:mm:ss}'", _val);
        }
    }
}