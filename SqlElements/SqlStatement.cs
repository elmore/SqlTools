using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.ObjectBuilder2;
using SqlElements.SqlTypes;

namespace SqlElements
{
    public abstract class SqlStatement
    {
        protected readonly Dictionary<string, SqlType> _values = new Dictionary<string, SqlType>();

        protected string ValueString
        {
            get
            {
                return _values.Values.Select(x => x.ToString()).JoinStrings(", ");
            }
        }

        protected string FieldString
        {
            get
            {
                return _values.Keys.JoinStrings(", ");
            }
        }

        public void AddParameterRange(IDictionary<string, object> values)
        {
            values.ForEach(pair => _values.Add(pair.Key, SqlType.Create(pair.Value)));
        }

        public void AddParameter(string key, object val)
        {
            _values.Add(key, SqlType.Create(val));
        }
    }
}