using System;
using System.Collections.Generic;
using System.Text;

using AutoMapper;

namespace D3K.Diagnostics.Common
{
    public interface ILogValueMapperConfigurator
    {
        void Configurate(IMapperConfigurationExpression mapperConfigurationExpression);
    }
}
