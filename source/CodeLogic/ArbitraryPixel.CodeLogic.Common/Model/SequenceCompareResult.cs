using System;
using System.Text;

namespace ArbitraryPixel.CodeLogic.Common.Model
{
    public interface ISequenceCompareResult
    {
        SequenceIndexCompareResult[] Result { get; }
        bool IsEqual { get; }
    }

    public struct SequenceCompareResult : ISequenceCompareResult
    {
        private SequenceIndexCompareResult[] _result;

        public SequenceCompareResult(SequenceIndexCompareResult[] result)
        {
            _result = result ?? throw new ArgumentNullException();
        }

        public bool IsEqual
        {
            get
            {
                bool equal = (_result.Length > 0) ? true : false;

                if (equal == true)
                {
                    foreach (SequenceIndexCompareResult r in _result)
                    {
                        if (r != SequenceIndexCompareResult.Equal)
                        {
                            equal = false;
                            break;
                        }
                    }
                }

                return equal;
            }
        }

        public SequenceIndexCompareResult[] Result => _result;

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("{");
            for (int i = 0; i < _result.Length; i++)
            {
                if (i != 0)
                    sb.Append(", ");
                sb.Append(_result[i].ToString());
            }
            sb.Append("}");

            return sb.ToString();
        }
    }
}
