using System;
using System.Linq;
using Microsoft.Practices.ObjectBuilder2;

namespace SqlElements
{
    public class SqlExec : SqlStatement
    {
        protected readonly string _commandName;

        public SqlExec(string commandName)
        {
            _commandName = commandName;
        }

        public override string ToString()
        {
            string parameters = _values.Select(x => String.Format("{0} = {1}", x.Key, x.Value)).JoinStrings(", ");

            return String.Format("exec {0} {1}", _commandName, parameters);
        }
    }
}
