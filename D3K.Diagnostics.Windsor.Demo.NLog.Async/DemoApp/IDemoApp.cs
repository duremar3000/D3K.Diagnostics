using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace D3K.Diagnostics.Windsor.Demo.NLog.Async
{
    public interface IDemoApp
    {
        Task RunAsync();
    }
}
