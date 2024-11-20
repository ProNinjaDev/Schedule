using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab_work_inclass
{
    internal class RegularGroup : Group
    {
        public RegularGroup(int number, List<Subject> subjects) : base(number, subjects) { }

        public override void AssignLecture(int day, int pair, Subject subject, Lectoriy lectory)
        {
            if (RemainingLectures[subject] > 0)
            {
                Schedule[day][pair] = subject.Name;
                AssignedLectories[day][pair] = lectory.Number;
                RemainingLectures[subject]--;
            }
        }

        public override void AssignPractice(int day, int pair, Subject subject, Terminal terminal)
        {
            if (RemainingPractices[subject] > 0)
            {
                Schedule[day][pair] = subject.Name;
                AssignedTerminals[day][pair] = terminal.Number;
                RemainingPractices[subject]--;
            }
        }

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

        public override Subject FindPractice()
        {
            foreach (var dataPractice in RemainingPractices)
            {
                if (dataPractice.Value > 0)
                {
                    return dataPractice.Key;
                }

            }
            return null;
        }

        public override string GetGroupType()
        {
            return "ЛП";
        }
    }
}
