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

        public Dictionary<int, List<string>> Schedule { get; set; }

        public Dictionary<int, List<int>> AssignedLectories { get; set; }
        public Dictionary<int, List<int>> AssignedTerminals { get; set; }
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

            Schedule = new Dictionary<int, List<string>>();
            AssignedLectories = new Dictionary<int, List<int>>();
            AssignedTerminals = new Dictionary<int, List<int>>();

            for (int i = 0; i < Program.NUMDAYS; i++)
            {
                Schedule[i] = new List<string>(new string[Program.NUMPAIRS]);
                AssignedLectories[i] = new List<int>(Program.NUMPAIRS);
                AssignedLectories[i].AddRange(Enumerable.Repeat(-1, Program.NUMPAIRS));

                AssignedTerminals[i] = new List<int>(Program.NUMPAIRS);
                AssignedTerminals[i].AddRange(Enumerable.Repeat(-1, Program.NUMPAIRS));
            }
        }

        public void AssignLecture(int day, int pair, Subject subject, Lectoriy lectory)
        {
            if (RemainingLectures[subject] > 0)
            {
                Schedule[day][pair] = subject.Name;
                AssignedLectories[day][pair] = lectory.Number; // Присваиваем аудиторию для лекции
                RemainingLectures[subject]--;
            }
        }

        public void AssignPractice(int day, int pair, Subject subject, Terminal terminal)
        {
            if (RemainingPractices[subject] > 0)
            {
                Schedule[day][pair] = subject.Name;
                AssignedTerminals[day][pair] = terminal.Number; // Присваиваем терминал для практики
                RemainingPractices[subject]--;
            }
        }

        public void AssignMilitaryDay(int day)
        {
            for(int pair = 0; pair < Program.MAXPAIRS; pair++)
            {
                Schedule[day][pair] = "Военная подготовка";
            }
        }

        public bool IsFreeDay(int day)
        {
            return Schedule[day].Count == 0;
        }

        public bool IsMilitaryDay(int day)
        {
            return Schedule[day].Contains("Военная подготовка");
        }

        public Subject FindLecture()
        {
            foreach (var dataLecture in RemainingLectures)
            {
                if(dataLecture.Value > 0)
                {
                    return dataLecture.Key;
                }

            }
            return null;
        }

        public Subject FindPractice()
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

        public int GetNumPairs(int day)
        {
            return Schedule[day].Count(x => x != null);
        }

        // todo: Переработать взятие лекций и практик из расписания
        public string GetLecture(int day, int pair)
        {
            return Schedule[day][pair];
        }

        public string GetPractice(int day, int pair)
        {
            return Schedule[day][pair];
        }

        public int GetAssignedLectory(int day, int pair)
        {
            return AssignedLectories[day][pair];
        }

        public int GetAssignedTerminal(int day, int pair)
        {
            return AssignedTerminals[day][pair];
        }
    }
}
