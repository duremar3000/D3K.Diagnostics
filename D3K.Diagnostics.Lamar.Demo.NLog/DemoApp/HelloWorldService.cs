using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace D3K.Diagnostics.Demo
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

        public HelloWorldModel GetHelloWorld()
        {
            return new HelloWorldModel
            {
                Hello = _helloService.GetHello(),
                World = _worldService.GetWorld()
            };
        }
    }
}
