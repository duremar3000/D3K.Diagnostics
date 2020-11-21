using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace D3K.Diagnostics.Common
{
    public class MethodIdentity
    {
        readonly string[] _chein;
        readonly string _str;

        private MethodIdentity(IEnumerable<string> chein)
        {
            if (chein == null)
                throw new ArgumentNullException();

            _chein = chein.ToArray();

            _str = string.Join(" ", _chein);
        }

        public MethodIdentity GetNext()
        {
            var tail = Guid.NewGuid().ToString().GetHashCode().ToString("x").PadLeft(8, 'x');

            var next = _chein.ToList();
            next.Add(tail);

            return new MethodIdentity(next);
        }

        public override string ToString()
        {
            return _str;
        }

        public static MethodIdentity Create(string str = null)
        {
            var chein = str?.Split(' ').Where(i => i != "") ?? new string[] { };

            var curr = new MethodIdentity(chein);

            return curr;
        }
    }
}
