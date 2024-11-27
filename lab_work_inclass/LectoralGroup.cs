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

        public void AssignLecture(int day, int pair, Subject subject, Lectoriy lectory)
        {
            if (RemainingLectures[subject] > 0)
            {
                Schedule[day][pair] = subject.Name;
                AssignedLectories[day][pair] = lectory.Number;
                RemainingLectures[subject]--;
            }
        }


        public Subject FindLecture()
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

        public override string GetGroupType()
        {
            return "Л";
        }
    }
}
