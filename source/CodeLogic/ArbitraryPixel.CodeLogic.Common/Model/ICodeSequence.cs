using ArbitraryPixel.Common;

namespace ArbitraryPixel.CodeLogic.Common.Model
{
    public interface ICodeSequence
    {
        CodeValue this[int index] { get; set; }

        CodeValue[] Code { get; }
        int Length { get; }

        bool Contains(CodeValue codeValue);
        void GenerateRandomCode(IRandom r);
        void SetCode(CodeValue[] code);
    }
}