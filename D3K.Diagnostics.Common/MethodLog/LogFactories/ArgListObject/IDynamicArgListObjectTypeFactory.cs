using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace D3K.Diagnostics.Common
{
    public interface IDynamicArgListObjectTypeFactory
    {
        Type CreateDynamicArgListObjectType(MethodInfo methodInfo);
    }
}
