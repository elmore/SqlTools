namespace SqlElements.SqlTypes
{
    public class SqlObject : SqlType
    {
        public SqlObject(object val) : base(val) { }

        public override string ToString()
        {
            return _val.ToString();
        }
    }
}