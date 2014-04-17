using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlTools
{
    class SystemConsole : IOutput
    {
        public void Write(string line)
        {
            Console.Write(line);
        }

        public void WriteLine(string line)
        {
            Console.WriteLine(line);
        }
    }
}
