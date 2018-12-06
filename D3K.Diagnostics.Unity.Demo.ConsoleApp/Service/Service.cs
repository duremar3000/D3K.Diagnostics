using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D3K.Diagnostics.Unity.Demo.ConsoleApp
{
    public class Service : IService
    {
        public DoWorkResult DoWork(DoWorkArgs args)
        {
            return new DoWorkResult { Value = args.Arg1 + args.Arg2 };
        }
    }
}
