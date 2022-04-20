using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Serilog.Core;
using Serilog.Events;

namespace D3K.Diagnostics.SerilogExtensions
{
    public class RemoveTypeTagEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var properties = logEvent.Properties.ToArray();

            foreach (var property in properties)
            {
                if (property.Value is StructureValue value)
                {
                    value = RemoveTypeTag(value);

                    logEvent.AddOrUpdateProperty(new LogEventProperty(property.Key, value));
                }
            }
        }

        private StructureValue RemoveTypeTag(StructureValue value)
        {
            return new StructureValue(RemoveTypeTag(value.Properties), typeTag: null);
        }

        private IEnumerable<LogEventProperty> RemoveTypeTag(IEnumerable<LogEventProperty> properties)
        {
            foreach (var property in properties)
            {
                if (property.Value is StructureValue value)
                    yield return new LogEventProperty(property.Name, RemoveTypeTag(value));
                else
                    yield return property;
            }
        }
    }
}
