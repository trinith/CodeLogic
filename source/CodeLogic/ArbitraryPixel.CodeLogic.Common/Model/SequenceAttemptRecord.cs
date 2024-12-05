using System;
using System.Text;

namespace ArbitraryPixel.CodeLogic.Common.Model
{
    public interface ISequenceAttemptRecord
    {
        CodeValue[] Code { get; }
        SequenceIndexCompareResult[] Result { get; }
    }

    public struct SequenceAttemptRecord : ISequenceAttemptRecord
    {
        public CodeValue[] Code { get; private set; }
        public SequenceIndexCompareResult[] Result { get; private set; }

        public SequenceAttemptRecord(CodeValue[] code, SequenceIndexCompareResult[] result)
            : this()
        {
            this.Code = code ?? throw new ArgumentNullException();
            this.Result = result ?? throw new ArgumentNullException();

            if (this.Code.Length == 0 || this.Result.Length == 0)
                throw new ArgumentException();
        }
    }
}
