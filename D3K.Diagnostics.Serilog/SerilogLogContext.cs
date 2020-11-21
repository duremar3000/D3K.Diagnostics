using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading;

using Serilog.Context;

using D3K.Diagnostics.Core;

namespace D3K.Diagnostics.SerilogExtensions
{
    public class SerilogLogContext : ILogContext
    {
        static readonly AsyncLocal<Dictionary<string, object>> _items = new AsyncLocal<Dictionary<string, object>>();

        private static Dictionary<string, object> Items
        {
            get => _items.Value == null ? _items.Value = new Dictionary<string, object>() : _items.Value;
            set => _items.Value = value;
        }

        public object PeekProperty(string name)
        {
            Items.TryGetValue(name, out object value);

            return value;
        }

        public IDisposable PushProperty(string name, object value)
        {
            var bookmark = Items.ToDictionary(i => i.Key, i => i.Value);

            var logContextPop = LogContext.PushProperty(name, value);

            var items = Items.ToDictionary(i => i.Key, i => i.Value);
            items[name] = value;

            Items = items;

            return new PropertyScope(bookmark, logContextPop);
        }

        class PropertyScope : IDisposable
        {
            readonly Dictionary<string, object> _bookmark;
            readonly IDisposable _logContextPop;

            public PropertyScope(Dictionary<string, object> bookmark, IDisposable logContextPop)
            {
                _bookmark = bookmark ?? throw new ArgumentNullException();
                _logContextPop = logContextPop ?? throw new ArgumentNullException();
            }

            public void Dispose()
            {
                Items = _bookmark;

                _logContextPop.Dispose();
            }
        }
    }
}
