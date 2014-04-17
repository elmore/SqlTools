using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlTools
{
    class SqlToolsApp
    {
        public SqlToolsApp(IOutput console, string[] args)
        {
            if (args.Length == 0)
            {
                console.WriteLine("You need at least one argument");
                return;
            }

            try
            {
                IMethod meth = MethodFactory.GetMethod(console, args[0]);

                meth.Run(args.Skip(1).Take(args.Length - 1).ToArray());
            }
            catch (Exception e)
            {
                console.Write(e.ToString());
            }
        }
    }
}
