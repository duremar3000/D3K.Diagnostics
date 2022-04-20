using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace D3K.Diagnostics.Demo
{
    public class DemoApp : IDemoApp
    {
        readonly IHelloWorldService _helloWorldService;
        readonly IPrinter _printer;

        public DemoApp(IHelloWorldService helloWorldService, IPrinter printer)
        {
            _helloWorldService = helloWorldService;
            _printer = printer;
        }

        public async Task RunAsync()
        {
            var helloWorld = await _helloWorldService.GetHelloWorld();

            _printer.Print(helloWorld);
        }
    }
}
