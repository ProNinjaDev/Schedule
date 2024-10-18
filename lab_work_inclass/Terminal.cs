using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab_work_inclass
{
    internal class Terminal : IAudience
    {
        public int Number { get; }
        public List<List<bool>> Employment { get; set; }
        
        public Terminal(int number)
        {
            this.Number = number;

            this.Employment = new List<List<bool>>();
            for (int i = 0; i < Program.NUMDAYS; i++)
            {
                this.Employment.Add(new List<bool>());
                for (int j = 0; j < Program.NUMPAIRS; j++)
                {
                    this.Employment[i].Add(false);
                }
            }
        }

        public bool IsAvailable (int day, int pair)
        {
            return !Employment[day][pair];
        }
    }
}
