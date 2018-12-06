using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D3K.Diagnostics.Unity.Demo.ConsoleApp
{
    public interface IService
    {
        DoWorkResult DoWork(DoWorkArgs args);
    }

    public class DoWorkArgs
    {
        public string Arg1 { get; set; }

        public string Arg2 { get; set; }
    }

    public class DoWorkResult
    {
        public string Value { get; set; }
    }
}
