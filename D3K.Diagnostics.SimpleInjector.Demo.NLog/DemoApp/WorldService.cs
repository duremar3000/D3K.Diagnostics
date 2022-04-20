using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace D3K.Diagnostics.Demo
{
    public class WorldService : IWorldService
    {
        public WorldModel GetWorld()
        {
            return new WorldModel();
        }
    }
}
