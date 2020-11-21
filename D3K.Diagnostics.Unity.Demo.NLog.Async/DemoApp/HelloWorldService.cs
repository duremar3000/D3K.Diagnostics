using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace D3K.Diagnostics.Unity.Demo.NLog.Async
{
    public class HelloWorldService : IHelloWorldService
    {
        readonly IHelloService _helloService;
        readonly IWorldService _worldService;

        public HelloWorldService(IHelloService helloService, IWorldService worldService)
        {
            _helloService = helloService;
            _worldService = worldService;
        }

        public async Task<string> GetHelloWorld()
        {
            var helloTask = _helloService.GetHello();

            var worldTask = _worldService.GetWorld();

            var helloWorld = await Task.WhenAll(new[] { helloTask, worldTask });
            
            var res = string.Concat(helloWorld);

            return res;                 
        }
    }
}
