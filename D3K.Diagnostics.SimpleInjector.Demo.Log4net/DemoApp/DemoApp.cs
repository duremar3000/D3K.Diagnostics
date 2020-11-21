using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace D3K.Diagnostics.SimpleInjector.Demo.Log4net
{
    public class DemoApp : IDemoApp
    {
        readonly IHelloWorldService _helloWorldService;

        public DemoApp(IHelloWorldService helloWorldService)
        {
            _helloWorldService = helloWorldService;
        }

        public void Run()
        {
            var helloWorld = _helloWorldService.GetHelloWorld();

            Console.WriteLine(helloWorld);
        }
    }
}
