using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

using AutoMapper;

namespace D3K.Diagnostics.Unity.Log
{
    public class LogValueMapper : ILogValueMapper
    {
        readonly IMapper _autoMapper;

        public LogValueMapper()
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<byte[], object>().ConstructUsing(i => "byte[]");
                cfg.CreateMap<Delegate, object>().ConstructUsing(i => "function");
                cfg.CreateMap<string, object>().ConstructUsing(i => i);
                cfg.CreateMap<IEnumerable, object>().ConstructUsing(i => i.Cast<object>().Take(255));
            });

            _autoMapper = config.CreateMapper();
        }

        public object Map(object sourceLogValue)
        {
            if (sourceLogValue == null)
                return "Null";

            var targetTraceValue = _autoMapper.Map<object>(sourceLogValue);

            return targetTraceValue;
        }
    }
}
