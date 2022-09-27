using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq.Dynamic.Core;
using System.Linq;
using System.Text;

namespace D3K.Diagnostics.Common
{
    public class DynamicArgListObjectFactory : IDynamicArgListObjectFactory
    {
        readonly IDynamicArgListObjectTypeFactory _dynamicArgListObjectTypeFactory;

        public DynamicArgListObjectFactory(IDynamicArgListObjectTypeFactory dynamicArgListObjectTypeFactory)
        {
            _dynamicArgListObjectTypeFactory = dynamicArgListObjectTypeFactory ?? throw new ArgumentNullException();
        }

        public DynamicClass CreateArgListObject(MethodInfo methodInfo)
        {
            var type = _dynamicArgListObjectTypeFactory.CreateDynamicArgListObjectType(methodInfo);

            var obj = Activator.CreateInstance(type) as DynamicClass;

            return obj;
        }
    }
}
