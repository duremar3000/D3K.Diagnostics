using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace D3K.Diagnostics.SimpleInjector.Demo.Serilog.Async
{
    public class WorldService : IWorldService
    {
        public async Task<string> GetWorld()
        {
            await Task.Delay(5000);

            return "World";
        }
    }
}
