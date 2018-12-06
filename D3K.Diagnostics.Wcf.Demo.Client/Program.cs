using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using D3K.Diagnostics.Wcf.Demo.Client.ServiceReference;

namespace D3K.Diagnostics.Wcf.Demo.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var service = new ServiceClient();

            var res = service.DoWork(new DoWorkArgs { Arg1 = "Hello", Arg2 = "World" });

            Console.WriteLine(res.Value);

            service.DoWork2();

            Console.ReadLine();
        }
    }
}
