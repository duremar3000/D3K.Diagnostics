using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;

namespace D3K.Diagnostics.Wcf
{
    public class OperationIdentificationMessageInspector : IDispatchMessageInspector
    {
        readonly string _operationIdentityKey;

        public OperationIdentificationMessageInspector()
        {
            _operationIdentityKey = ConfigurationManager.AppSettings["OperationIdentityKey"];
        }

        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            return log4net.ThreadContext.Properties[_operationIdentityKey];
        }

        public void BeforeSendReply(ref Message reply, object correlationState)
        {
            var oid = correlationState;
            if (oid == log4net.ThreadContext.Properties[_operationIdentityKey])
            {
                if (reply == null) //oneway binding
                {
                    oid = null; 
                }
                else
                {
                    oid = $"{oid}[end]";
                }

                log4net.ThreadContext.Properties[_operationIdentityKey] = oid;
            }
        }
    }
}
