using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace D3K.Diagnostics.LightInject.Demo.NLog.Async
{
    public interface IWorldService
    {
        Task<string> GetWorld();
    }
}
