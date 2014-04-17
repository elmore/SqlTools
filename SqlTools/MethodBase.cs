using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlTools
{
    abstract class MethodBase : IMethod
    {
        protected IOutput _Console;

        public MethodBase(IOutput console)
        {
            _Console = console;
        }

        public abstract void Run(string[] args);
    }
}
