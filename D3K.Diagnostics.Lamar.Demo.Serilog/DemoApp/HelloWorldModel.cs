using System;
using System.Collections.Generic;
using System.Text;

namespace D3K.Diagnostics.Demo
{
    public class HelloWorldModel
    {
        public HelloModel Hello { get; set; }

        public WorldModel World { get; set; }

        public override string ToString()
        {
            return $"{Hello.Hello} {World.World}!";
        }
    }
}
