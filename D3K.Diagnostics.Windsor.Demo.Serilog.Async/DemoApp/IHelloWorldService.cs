using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace D3K.Diagnostics.Demo
{
    public interface IHelloWorldService
    {
        Task<HelloWorldModel> GetHelloWorld();
    }
}
