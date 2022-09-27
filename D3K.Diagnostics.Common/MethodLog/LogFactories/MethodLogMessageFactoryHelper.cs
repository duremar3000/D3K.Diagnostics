using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;
using AutoMapper;
using Newtonsoft.Json;

namespace D3K.Diagnostics.Common
{
    internal static class MethodLogMessageFactoryHelper
    {
        public static Type GetTargetType(MethodInput input)
        {
            Type targetType;
            if (input.Method.DeclaringType.IsAbstract)
            {
                targetType = input.TargetType;
            }
            else
            {
                targetType = input.Method.DeclaringType;
            }

            return targetType;
        }
    }
}
