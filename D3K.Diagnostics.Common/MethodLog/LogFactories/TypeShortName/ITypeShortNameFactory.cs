using System;
using System.Collections.Generic;
using System.Text;

namespace D3K.Diagnostics.Common
{
    public interface ITypeShortNameFactory
    {
        string CreateTypeShortName(Type type);
    }
}
