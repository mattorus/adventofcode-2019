using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2019.Tools
{
    public class Generator
    {
        public static List<List<int>> GeneratePhaseSequences(int start, int end)
        {
            List<List<int>> sequences = new List<List<int>>();

            for (int i = start; i < end; i++)
            {
                sequences.Add(new List<int> { i });
            }

            for (int i = start; i < end; i++)
            {
                List<List<int>> tempSequences = new List<List<int>>();

                foreach (var sequence in sequences)
                {   
                    for (int k = start; k < end; k++)
                    {
                        if (!sequence.Contains(k))
                        {
                            tempSequences.Add(new List<int>(sequence) { k });
                        }
                    }
                }

                if (tempSequences.Count > 0)
                {
                    sequences = new List<List<int>>(tempSequences);
                }
            }

            return sequences;
        }
    }
}
