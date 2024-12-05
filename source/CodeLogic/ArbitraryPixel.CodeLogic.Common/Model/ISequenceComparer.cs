namespace ArbitraryPixel.CodeLogic.Common.Model
{
    public interface ISequenceComparer
    {
        ISequenceCompareResult Compare(ICodeSequence testSequence, ICodeSequence masterSequence);
    }
}