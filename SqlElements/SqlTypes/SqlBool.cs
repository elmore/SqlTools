namespace SqlElements.SqlTypes
{
    public class SqlBool : SqlType
    {
        public SqlBool(bool val) : base(val) { }

        public override string ToString()
        {
            return ((bool)_val ? 1 : 0).ToString();
        }
    }
}