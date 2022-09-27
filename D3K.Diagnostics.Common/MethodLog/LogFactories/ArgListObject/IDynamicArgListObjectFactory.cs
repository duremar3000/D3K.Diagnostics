using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text;

namespace D3K.Diagnostics.Common
{
    public interface IDynamicArgListObjectFactory
    {
        DynamicClass CreateArgListObject(MethodInfo methodInfo);
    }
}
