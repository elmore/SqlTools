using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlTools
{
    class MethodFactory
    {
        public static IMethod GetMethod(IOutput console, string methodName)
        {
            
            // reflect this?
            switch (methodName.Trim().ToLower())
            {
                case "insert2update":
                    return new Insert2Update(console);

                case "runfile":
                    return new RunFile(console);

                case "runfiles":
                    return new RunFiles(console);

                default:
                    console.WriteLine(string.Format("The method '{0}' was not recognised", methodName));
                    return null;
            } 
        }
    }
}
