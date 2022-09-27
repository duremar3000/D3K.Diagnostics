using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace D3K.Diagnostics.Common
{
    public class TypeShortNameFactory : ITypeShortNameFactory
    {
        public string CreateTypeShortName(Type type)
        {
            var res = type.ToString();
            var namespaces = new List<string> { type.Namespace };

            if (type.IsGenericType)
            {
                var genericTypeArguments = type.GenericTypeArguments;

                for (int num = 0; num < genericTypeArguments.Length; num++)
                {
                    namespaces.Add(genericTypeArguments[num].Namespace);
                }
            }

            var items = namespaces.OrderByDescending(i => i?.Count() ?? 0);

            foreach (string item in items)
            {
                res = res.Replace(string.Format("{0}.", item), "");
            }

            return res;
        }
    }
}
