using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace D3K.Diagnostics.LightInject.Demo.Log4net.Async
{
    public interface IWorldService
    {
        Task<string> GetWorld();
    }
}
