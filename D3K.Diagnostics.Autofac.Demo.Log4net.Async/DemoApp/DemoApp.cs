using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace D3K.Diagnostics.Autofac.Demo.Log4net.Async
{
    public class DemoApp : IDemoApp
    {
        readonly IHelloWorldService _helloWorldService;

        public DemoApp(IHelloWorldService helloWorldService)
        {
            _helloWorldService = helloWorldService;
        }

        public async Task RunAsync()
        {
            var helloWorld = await _helloWorldService.GetHelloWorld();

            Console.WriteLine(helloWorld);
        }
    }
}
