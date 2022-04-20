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

        public async Task<HelloWorldModel> GetHelloWorld()
        {
            var helloTask = _helloService.GetHello();

            var worldTask = _worldService.GetWorld();

            await Task.WhenAll(helloTask, worldTask);

            var hello = await helloTask;

            var world = await worldTask;

            return new HelloWorldModel { Hello = hello, World = world };
        }
    }
}
