using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.IO;

using AutoMapper;

namespace D3K.Diagnostics.Unity
{
    public class DefaultLogValueMapperConfigurator : ILogValueMapperConfigurator
    {
        public void Configurate(IMapperConfigurationExpression mapperConfigurationExpression)
        {
            mapperConfigurationExpression.CreateMap<byte[], object>().ConstructUsing(i => "byte[]");
            mapperConfigurationExpression.CreateMap<Stream, object>().ConstructUsing(i => "Stream");
            mapperConfigurationExpression.CreateMap<Delegate, object>().ConstructUsing(i => "function");
            mapperConfigurationExpression.CreateMap<string, object>().ConstructUsing(i => i);
            mapperConfigurationExpression.CreateMap<IEnumerable, object>().ConstructUsing(i => i.Cast<object>().Take(255));
        }
    }
}
