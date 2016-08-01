using System;

namespace SqlElements.SqlTypes
{
    public abstract class SqlType
    {
        protected readonly object _val;

        protected SqlType(object val)
        {
            _val = val;
        }

        public static SqlType Create(object value)
        {
            if (value is SqlType)
            {
                return (SqlType)value;
            }

            if (value == null || value == DBNull.Value)
            {
                return new SqlNull();
            }

            if (value is int)
            {
                return new SqlInt((int) value);
            }

            if(value is byte)
            {
                return new SqlByte((byte)value);
            }

            if (value is bool)
            {
                return new SqlBool((bool) value);
            }

            if (value is DateTime)
            {
                return new SqlDateTime((DateTime)value);
            }

            if (value is String)
            {
                return new SqlVarChar((string)value);
            }

            return new SqlObject(value);
        }
    }
}