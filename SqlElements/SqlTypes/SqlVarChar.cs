
namespace SqlElements.SqlTypes
{
    public class SqlVarChar : SqlType
    {
        public SqlVarChar(string val) : base(val.Replace("'", "''")) { }

        public override string ToString()
        {
            return string.Format("'{0}'", _val);
        }
    }
}