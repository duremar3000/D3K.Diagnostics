using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace D3K.Diagnostics.Unity
{
    public interface ILogValueMapperConfigurator
    {
        void Configurate(IMapperConfigurationExpression mapperConfigurationExpression);
    }
}
