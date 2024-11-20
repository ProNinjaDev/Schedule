using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab_work_inclass
{
    internal class Subject
    {
        public string Name { get; }
        public int numLectures { get; }
        public int numPractices { get; }

        public Subject(string name, int numLectures, int numPractices) 
        {
            Name = name;
            this.numLectures = numLectures;
            this.numPractices = numPractices;
        }
    }
}
