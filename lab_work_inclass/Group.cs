using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab_work_inclass
{
    internal class Group
    {
        public int Number {  get; }
        public Dictionary<Subject, int> RemainingLectures { get; set; }
        public Dictionary<Subject, int> RemainingPractices { get; set; }

        public Group(int number, List<Subject> subjects) 
        {
            this.Number = number;

            RemainingLectures = new Dictionary<Subject, int>();
            RemainingPractices = new Dictionary<Subject, int>();

            foreach(var subject in subjects)
            {
                RemainingLectures[subject] = subject.numLectures;
                RemainingPractices[subject] = subject.numPractices;
            }
        }
    }
}
