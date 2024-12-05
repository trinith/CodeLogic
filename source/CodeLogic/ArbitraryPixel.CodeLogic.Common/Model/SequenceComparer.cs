using System;
using System.Collections.Generic;
using System.Linq;

namespace ArbitraryPixel.CodeLogic.Common.Model
{
    public class SequenceComparer : ISequenceComparer
    {
        public ISequenceCompareResult Compare(ICodeSequence testSequence, ICodeSequence masterSequence)
        {
            List<SequenceIndexCompareResult> indexCompare = new List<SequenceIndexCompareResult>();

            if (testSequence != null && masterSequence != null && testSequence.Length == masterSequence.Length)
            {
                List<int> indexSet = Enumerable.Range(0, masterSequence.Length).ToList();
                List<int> equalIndexes = new List<int>();
                int[] counts = new int[Enum.GetValues(typeof(CodeValue)).Length];
                for (int i = 0; i < masterSequence.Length; i++)
                    counts[(int)masterSequence[i]]++;

                // First pass, find equal, remove if found.
                for (int i = 0; i < indexSet.Count; i++)
                {
                    int index = indexSet[i];
                    if (testSequence[index] == masterSequence[index])
                    {
                        indexCompare.Add(SequenceIndexCompareResult.Equal);

                        indexSet.RemoveAt(i);
                        equalIndexes.Add(index);
                        counts[(int)masterSequence[index]]--;
                        i--;
                    }
                }

                // Second pass, look for partials
                for (int i = 0; i < indexSet.Count; i++)
                {
                    int index = indexSet[i];
                    if (masterSequence.Contains(testSequence[index]) && counts[(int)testSequence[index]] > 0)
                    {
                        indexCompare.Add(SequenceIndexCompareResult.PartialEqual);
                        counts[(int)testSequence[index]]--;
                    }
                }
            }

            if (masterSequence != null && indexCompare.Count < masterSequence.Length)
            {
                int padAmount = masterSequence.Length - indexCompare.Count;
                for (int i = 0; i < padAmount; i++)
                    indexCompare.Add(SequenceIndexCompareResult.NotEqual);
            }

            return new SequenceCompareResult(indexCompare.ToArray());
        }
    }
}
