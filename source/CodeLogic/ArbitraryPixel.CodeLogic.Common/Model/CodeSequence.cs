using ArbitraryPixel.Common;
using System;
using System.Linq;
using System.Text;

namespace ArbitraryPixel.CodeLogic.Common.Model
{
    public class CodeSequence : ICodeSequence
    {
        private CodeValue[] _code = null;

        public int Length => _code.Length;
        public CodeValue[] Code => _code;

        public CodeValue this[int index]
        {
            get { return _code[index]; }
            set { _code[index] = value; }
        }

        public CodeSequence(int length)
        {
            if (length <= 0)
                throw new ArgumentException("Length must be greater than zero.");

            _code = new CodeValue[length];

            for (int i = 0; i < _code.Length; i++)
                _code[i] = (CodeValue)0;
        }

        public void SetCode(CodeValue[] code)
        {
            if (code == null)
                throw new ArgumentNullException();
            else if (code.Length <= 0)
                throw new ArgumentException();

            _code = code;
        }

        public void GenerateRandomCode(IRandom r)
        {
            if (r == null)
                throw new ArgumentNullException();

            int max = Enum.GetValues(typeof(CodeValue)).Length;

            for (int i = 0; i < _code.Length; i++)
                _code[i] = (CodeValue)r.Next(0, max);
        }

        public bool Contains(CodeValue value)
        {
            return _code.ToList().Contains(value);
        }

        public override string ToString()
        {
            StringBuilder output = new StringBuilder();
            output.Append("{");

            for (int i = 0; i < _code.Length; i++)
            {
                if (i != 0)
                    output.Append(", ");

                output.Append(_code[i].ToString());
            }
            output.Append("}");

            return output.ToString();
        }
    }
}
