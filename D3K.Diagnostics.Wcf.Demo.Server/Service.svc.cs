using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading;

namespace D3K.Diagnostics.Wcf.Demo.Server
{
    public class Service : IService
    {
        public DoWorkResult DoWork(DoWorkArgs args)
        {
            return new DoWorkResult { Value = args.Arg1 + args.Arg2 };
        }

        public void DoWork()
        {
            Thread.Sleep(5000);
        }
    }
}
