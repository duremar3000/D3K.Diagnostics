using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace D3K.Diagnostics.Demo
{
    public class WorldService : IWorldService
    {
        public async Task<WorldModel> GetWorld()
        {
            await Task.Delay(5000);

            return new WorldModel();
        }
    }
}
