using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace D3K.Diagnostics.SimpleInjector.Demo.Serilog.Async
{
    public interface IHelloWorldService
    {
        Task<string> GetHelloWorld();
    }
}
