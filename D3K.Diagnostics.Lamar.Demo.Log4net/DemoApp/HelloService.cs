using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace D3K.Diagnostics.Lamar.Demo.Log4net
{
    public class HelloService : IHelloService
    {
        public string GetHello()
        {
            return "Hello";
        }
    }
}
