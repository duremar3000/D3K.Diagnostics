using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace D3K.Diagnostics.LightInject.Demo.NLog
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

        public string GetHelloWorld()
        {
            var hello = _helloService.GetHello();

            var world = _worldService.GetWorld();           

            return hello + world;
        }
    }
}
