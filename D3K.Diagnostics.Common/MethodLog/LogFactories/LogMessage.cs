using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Text.RegularExpressions;

using D3K.Diagnostics.Core;

using Newtonsoft.Json;

namespace D3K.Diagnostics.Common
{
    public class LogMessage : ILogMessage, IDictionary<string, object>
    {
        #region Fields

        static readonly JsonSerializerSettings _jsonSettings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
        readonly Dictionary<string, object> _properties = new Dictionary<string, object>();
        readonly StringDictionary _stringProperties = new StringDictionary();

        #endregion

        #region Constructors

        public LogMessage(string messageTemplate)
        {
            MessageTemplate = messageTemplate ?? throw new ArgumentNullException();
        }

        #endregion

        #region ILogMessage

        public string MessageTemplate { get; }

        public void AddMessageProperty(string propertyName, string propertyValue)
        {
            _properties[propertyName] = propertyValue;
            _stringProperties[propertyName] = propertyValue;
        }

        public void AddMessageProperty(string propertyName, object propertyValue)
        {
            _properties[propertyName] = propertyValue;
            _stringProperties[propertyName] = JsonConvert.SerializeObject(propertyValue, _jsonSettings);
        }

        public void AddMessageProperty(string propertyName, Exception propertyValue)
        {
            _properties[propertyName] = propertyValue;
            _stringProperties[propertyName] = propertyValue.ToString();
        }

        public void AddMessageProperty(string propertyName, int propertyValue)
        {
            _properties[propertyName] = propertyValue;
            _stringProperties[propertyName] = propertyValue.ToString();
        }

        public void AddMessageProperty(string propertyName, double propertyValue)
        {
            _properties[propertyName] = propertyValue;
            _stringProperties[propertyName] = propertyValue.ToString();
        }

        public void AddMessageProperty(string propertyName, DateTime propertyValue)
        {
            _properties[propertyName] = propertyValue;
            _stringProperties[propertyName] = propertyValue.ToString();
        }

        #endregion

        #region IDictionary<string, object>

        object IDictionary<string, object>.this[string key]
        {
            get => _properties[key];
            set => _properties[key] = value;
        }

        ICollection<string> IDictionary<string, object>.Keys => _properties.Keys;

        ICollection<object> IDictionary<string, object>.Values => _properties.Values;

        int ICollection<KeyValuePair<string, object>>.Count => _properties.Count;

        bool ICollection<KeyValuePair<string, object>>.IsReadOnly => false;

        void IDictionary<string, object>.Add(string key, object value)
        {
            AddMessageProperty(key, value);
        }

        void ICollection<KeyValuePair<string, object>>.Add(KeyValuePair<string, object> item)
        {
            ((ICollection<KeyValuePair<string, object>>)_properties).Add(item);
        }

        void ICollection<KeyValuePair<string, object>>.Clear()
        {
            _properties.Clear();
            _stringProperties.Clear();
        }

        bool ICollection<KeyValuePair<string, object>>.Contains(KeyValuePair<string, object> item)
        {
            return ((ICollection<KeyValuePair<string, object>>)_properties).Contains(item);
        }

        bool IDictionary<string, object>.ContainsKey(string key)
        {
            return _properties.ContainsKey(key);
        }

        void ICollection<KeyValuePair<string, object>>.CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        IEnumerator<KeyValuePair<string, object>> IEnumerable<KeyValuePair<string, object>>.GetEnumerator()
        {
            return _properties.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _properties.GetEnumerator();
        }

        bool IDictionary<string, object>.Remove(string key)
        {
            var res = _properties.Remove(key); 
            
            _stringProperties.Remove(key);

            return res;
        }

        bool ICollection<KeyValuePair<string, object>>.Remove(KeyValuePair<string, object> item)
        {
            return ((ICollection<KeyValuePair<string, object>>)_properties).Remove(item);
        }

        bool IDictionary<string, object>.TryGetValue(string key, out object value)
        {
            return _properties.TryGetValue(key, out value);
        }

        #endregion

        #region Methods

        public override string ToString()
        {
            var res = Regex.Replace(MessageTemplate, @"{{(?<propertyName>[^{]+)}}", match => _stringProperties[match.Groups["propertyName"].Value]);

            return res;
        }

        #endregion
    }
}
