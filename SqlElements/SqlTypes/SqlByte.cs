namespace SqlElements.SqlTypes
{
    public class SqlByte : SqlType
    {
        public SqlByte(byte val) : base(val) { }

        public override string ToString()
        {
            return _val.ToString();
        }
    }
}