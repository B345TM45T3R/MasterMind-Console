using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterMind___Console
{
    class Code
    {
        private int[] code;
        private static Random random = new Random();

        public Code(int CodeLength, int NumColours, bool allowBlanks)
        {
            int lowerBound = (allowBlanks) ? -1 : 0;

            code = new int[CodeLength];
            if (NumColours < 10)
                for (int x = 0; x< CodeLength; x++)
                {
                    code[x] = random.Next(lowerBound, NumColours);
                    // For all non-blank (i.e. not -1), add 1 to get 1-based, not 0-based index
                    if (code[x] != -1)
                    {
                        code[x] += 1;
                    }
                }
            else
                for (int x = 0; x < CodeLength; x++)
                {
                    code[x] = random.Next(lowerBound, NumColours); 
                    // 0 is the 10th colour, so don't need the + 1 offset
                }
        }

        public Code(int CodeLength)
        {
            code = new int[CodeLength];
            for (int x = 0; x < CodeLength; x++)
            {
                code[x] = 999;
            }
        }

        public void SubmitCode(int[] attemptBuilder)
        {
            //Convert from codeBuilder [1-indexed] to Code [0-indexed]
            for (int x = 1; x < attemptBuilder.Length; x++) // ignore first element - was the selected/active peg
            {
                code[x-1] = attemptBuilder[x];
            }
        }

        public int[] GetCode()
        {
            return code;
        }
    }
}
