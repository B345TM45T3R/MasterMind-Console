using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterMind___Console
{
    /// Excluded from project
    class Score
    {
        private string score;

        //Constructor
        public Score(int CodeLength, bool Secret = false)
        {
            score = "";
        }

        public void SubmitScore(string score)
        {
            this.score = score;           
        }

        public string GetScore()
        {
            return score;
        }
    }
}
