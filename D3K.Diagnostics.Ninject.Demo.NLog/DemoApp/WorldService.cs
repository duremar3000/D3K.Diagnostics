using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace D3K.Diagnostics.Ninject.Demo.NLog
{
    public class WorldService : IWorldService
    {
        public string GetWorld()
        {
            return "World";
        }
    }
}
