﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace D3K.Diagnostics.Windsor.Demo.Log4net.Async
{
    public interface IHelloService
    {
        Task<string> GetHello();
    }
}