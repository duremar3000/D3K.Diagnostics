using System;
using System.Collections.Generic;
using System.Text;

namespace D3K.Diagnostics.Common
{
    public interface IArgListObjectMapper
    {
        object Map(MethodInput input);
    }
}
