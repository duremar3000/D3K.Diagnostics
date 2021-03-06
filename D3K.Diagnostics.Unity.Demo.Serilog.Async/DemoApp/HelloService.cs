﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace D3K.Diagnostics.Unity.Demo.Serilog.Async
{
    public class HelloService : IHelloService
    {
        public async Task<string> GetHello()
        {
            await Task.Delay(5000);

            return "Hello";
        }
    }
}
