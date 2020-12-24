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

        public Code(int CodeLength, int NumColours)
        {
            code = new int[CodeLength];
            if (NumColours < 10)
                for (int x = 0; x< CodeLength; x++)
                {
                    code[x] = random.Next(1, NumColours + 1);
                }
            else
                for (int x = 0; x < CodeLength; x++)
                {
                    code[x] = random.Next(0, NumColours);
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

        public void SubmitCode(string attempt)
        {
            for (int x = 0; x < attempt.Length; x++)
            {
                code[x] = int.Parse(attempt.Substring(x,1));
            }
        }

        public int[] GetCode()
        {
            return code;
        }
    }
}
