using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace D3K.Diagnostics.Unity
{
    public class MethodIdentificator
    {
        readonly string[] _chein;
        readonly string _str;

        private MethodIdentificator(IEnumerable<string> chein)
        {
            if (chein == null)
                throw new ArgumentNullException();

            _chein = chein.ToArray();

            _str = string.Join("+", _chein);
        }

        public MethodIdentificator GetNext()
        {
            var tail = Guid.NewGuid().ToString().GetHashCode().ToString("x").PadLeft(8, 'x');

            var next = _chein.ToList();
            next.Add(tail);

            return new MethodIdentificator(next);
        }

        public override string ToString()
        {
            return _str;
        }

        public static MethodIdentificator Create(string str)
        {
            var chein = str?.Split('+').Where(i => i != "") ?? new string[] { };

            var curr = new MethodIdentificator(chein);

            return curr;
        }
    }
}
