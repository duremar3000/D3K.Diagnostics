using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace D3K.Diagnostics.Wcf.Demo.Server
{
    [ServiceContract]
    public interface IService
    {
        [OperationContract]
        DoWorkResult DoWork(DoWorkArgs args);

        [OperationContract(Name = "DoWork2", IsOneWay = true)]
        void DoWork();
    }

    [DataContract]
    public class DoWorkArgs
    {
        [DataMember]
        public string Arg1 { get; set; }

        [DataMember]
        public string Arg2 { get; set; }
    }

    [DataContract]
    public class DoWorkResult
    {
        [DataMember]
        public string Value { get; set; }
    }
}
