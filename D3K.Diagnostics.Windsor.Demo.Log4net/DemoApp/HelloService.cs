using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace D3K.Diagnostics.Demo
{
    public class HelloService : IHelloService
    {
        public HelloModel GetHello()
        {            
            return new HelloModel();
        }
    }
}
