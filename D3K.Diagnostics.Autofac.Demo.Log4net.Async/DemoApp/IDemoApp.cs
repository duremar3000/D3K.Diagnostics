﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace D3K.Diagnostics.Autofac.Demo.Log4net.Async
{
    public interface IDemoApp
    {
        Task RunAsync();
    }
}