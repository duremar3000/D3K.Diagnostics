using System;
using System.Collections.Generic;
using System.Text;

namespace D3K.Diagnostics.Demo
{
    public class Printer : IPrinter
    {
        public void Print(object arg)
        {
            Console.WriteLine(arg);
        }
    }
}
