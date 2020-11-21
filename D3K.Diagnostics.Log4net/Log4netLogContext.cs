using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using D3K.Diagnostics.Core;

namespace D3K.Diagnostics.Log4netExtensions
{
    public class Log4netLogContext : ILogContext
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

            log4net.LogicalThreadContext.Properties[name] = value;

            var items = Items.ToDictionary(i => i.Key, i => i.Value);
            items[name] = value;

            Items = items;

            return new PropertyScope(bookmark, name);
        }

        class PropertyScope : IDisposable
        {
            readonly Dictionary<string, object> _bookmark;
            readonly string _name;

            public PropertyScope(Dictionary<string, object> bookmark, string name)
            {
                _bookmark = bookmark ?? throw new ArgumentNullException();
                _name = name ?? throw new ArgumentNullException();
            }

            public void Dispose()
            {
                Items = _bookmark;

                Items.TryGetValue(_name, out object value);

                log4net.LogicalThreadContext.Properties[_name] = value;
            }
        }
    }
}
