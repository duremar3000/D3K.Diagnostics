using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

using AutoMapper;

namespace D3K.Diagnostics.Unity
{
    public class LogValueMapper : ILogValueMapper
    {
        readonly IMapper _autoMapper;

        public LogValueMapper(ILogValueMapperConfigurator logValueMapperConfigurator = null)
        {
            var config = new MapperConfiguration(cfg => {
                logValueMapperConfigurator?.Configurate(cfg);
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
