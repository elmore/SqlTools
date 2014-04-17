using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlTools
{
    class Program
    {
        static void Main(string[] args)
        {
            new SqlToolsApp(new SystemConsole(), args);
        }
    }
}
