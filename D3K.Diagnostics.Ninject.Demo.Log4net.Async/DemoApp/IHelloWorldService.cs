﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace D3K.Diagnostics.Ninject.Demo.Log4net.Async
{
    public interface IHelloWorldService
    {
        Task<string> GetHelloWorld();
    }
}
