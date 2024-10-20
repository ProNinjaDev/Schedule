﻿using System;
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

        //public int MilitaryDay { get; set; }

        public Dictionary<int, List<string>> Schedule { get; set; }

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

            for (int i = 0; i < Program.NUMDAYS; i++)
            {
                Schedule[i] = new List<string>(new string[Program.NUMPAIRS]);
            }
        }

        public void AssignLecture(int day, int pair, Subject subject)
        {
            if (RemainingLectures[subject] > 0)
            {
                Schedule[day][pair] = subject.Name;
                RemainingLectures[subject]--;
            }
        }

        public void AssignPractice(int day, int pair, Subject subject)
        {
            if (RemainingPractices[subject] > 0)
            {
                Schedule[day][pair] = subject.Name;
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
    }
}
