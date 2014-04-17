using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlTools
{
    interface IOutput
    {
        void Write(string line);
        void WriteLine(string line);
    }
}
