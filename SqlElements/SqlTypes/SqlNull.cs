namespace SqlElements.SqlTypes
{
    public class SqlNull : SqlType
    {
        public SqlNull() : base(null) { }

        public override string ToString()
        {
            return "null";
        }
    }
}