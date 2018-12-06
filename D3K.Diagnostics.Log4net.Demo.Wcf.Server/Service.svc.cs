using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace D3K.Diagnostics.Log4net.Demo.Wcf.Server
{
    public class Service : IService
    {
        public string DoWork(string arg1, string arg2)
        {
            return arg1 + arg2;
        }

        public DoWorkResult DoWork(DoWorkArgs args)
        {
            return new DoWorkResult { Value = args.Arg1 + args.Arg2 };
        }
    }
}
