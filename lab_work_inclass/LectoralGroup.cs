using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab_work_inclass
{
    internal class LectoralGroup : Group
    {
        public LectoralGroup(int number, List<Subject> subjects) : base(number, subjects) { }

        public override void AssignLecture(int day, int pair, Subject subject, Lectoriy lectory)
        {
            if (RemainingLectures[subject] > 0)
            {
                Schedule[day][pair] = subject.Name;
                AssignedLectories[day][pair] = lectory.Number;
                RemainingLectures[subject]--;
            }
        }

        public override void AssignPractice(int day, int pair, Subject subject, Terminal terminal) { }

        public override Subject FindLecture()
        {
            foreach (var dataLecture in RemainingLectures)
            {
                if (dataLecture.Value > 0)
                {
                    return dataLecture.Key;
                }

            }
            return null;
        }

        public override Subject FindPractice() => null;

        public override bool CanAttendLecture() => true;
        public override bool CanAttendPractice() => false;
    }
}
