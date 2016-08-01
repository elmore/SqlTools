namespace SqlElements
{
    public class SqlInsert : SqlStatement
    {
        protected readonly string _tableName;

        public SqlInsert(string tableName)
        {
            _tableName = tableName;
        }

        public override string ToString()
        {
            return string.Format("insert {0} ({1}) values ({2})", _tableName, FieldString, ValueString);
        }
    }
}