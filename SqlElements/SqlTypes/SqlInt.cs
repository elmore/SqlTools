namespace SqlElements.SqlTypes
{
    public class SqlInt : SqlType
    {
        public SqlInt(int val) : base(val) { }

        public override string ToString()
        {
            return _val.ToString();
        }
    }
}