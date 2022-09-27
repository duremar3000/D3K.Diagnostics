using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Linq.Dynamic.Core;
using System.Linq;

namespace D3K.Diagnostics.Common
{
    public class DynamicArgListObjectTypeFactory : IDynamicArgListObjectTypeFactory
    {
        public Type CreateDynamicArgListObjectType(MethodInfo methodInfo)
        {
            var properties = methodInfo.GetParameters()
                .Select(i => new DynamicProperty(i.Name, typeof(object))).ToArray();

            var type = DynamicClassFactory.CreateType(properties);

            return type;
        }
    }
}
