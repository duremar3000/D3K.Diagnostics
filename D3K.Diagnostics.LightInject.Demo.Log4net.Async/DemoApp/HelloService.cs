using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace D3K.Diagnostics.Demo
{
    public class HelloService : IHelloService
    {
        public async Task<HelloModel> GetHello()
        {
            await Task.Delay(5000);

            return new HelloModel();
        }
    }
}
