using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;

namespace D3K.Diagnostics.Common
{
    public class ArgListObjectMapper : IArgListObjectMapper
    {
        readonly ILogValueMapper _logValueMapper;
        readonly IDynamicArgListObjectFactory _dynamicArgListObjectFactory;

        public ArgListObjectMapper(
            ILogValueMapper logValueMapper,
            IDynamicArgListObjectFactory dynamicArgListObjectFactory)
        {
            _logValueMapper = logValueMapper ?? throw new ArgumentNullException();
            _dynamicArgListObjectFactory = dynamicArgListObjectFactory ?? throw new ArgumentNullException();
        }

        public object Map(MethodInput input)
        {
            if (input == null)
                throw new ArgumentNullException();

            var args = input.Method.GetParameters()
                .Select(i => new
                {
                    i.Name,
                    Value = _logValueMapper.Map(input.Arguments[i.Position])
                });

            var obj = _dynamicArgListObjectFactory.CreateArgListObject(input.Method);

            foreach (var arg in args)
            {
                obj.SetDynamicPropertyValue(arg.Name, arg.Value);
            }

            return obj;
        }
    }
}
