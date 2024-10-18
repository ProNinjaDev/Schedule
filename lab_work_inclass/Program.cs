using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Это снейк кейс ^-^
namespace lab_work_inclass
{
    
    internal class Program
    {
        internal const int NUMDAYS = 7;
        internal const int NUMPAIRS = 6;
        static void Main(string[] args)
        {
            int numGroups = 3;
            int numSubjects = 10;
            int numLections = 10;
            int numPractics = 5;
            int numRemainingsPairs = 4;
            int numLectRooms = 2;
            int numTermRooms = 1;

            List<Lectoriy> lectories = InitializeLectories(numLectRooms);
            List<Terminal> terminals = InitializeTerminals(numTermRooms);
            List<Subject> subjects = InitializeSubjects();
            List<Group> groups = InitializeGroups(numGroups, subjects);
        }

        static List<Lectoriy> InitializeLectories(int numLectRooms)
        {
            List<Lectoriy> lectories = new List<Lectoriy>();
            for (int i = 0; i < numLectRooms; i++)
            {
                lectories.Add(new Lectoriy(i + 1));
            }
            return lectories;
        }

        static List<Terminal> InitializeTerminals(int numTermRooms)
        {
            List<Terminal> terminals = new List<Terminal>();
            for (int i = 0; i < numTermRooms; i++)
            { 
                terminals.Add(new Terminal(i + 1));
            }
            return terminals;
        }

        static List<Subject> InitializeSubjects()
        {
            return new List<Subject>()
            {
                new Subject("Теория вычислимости", 1, 0),
                new Subject("Теория вероятностей", 1, 1),
                new Subject("Мат. основы автоматизации УПИМ", 1, 1),
                new Subject("КСЕ", 1, 0),
                new Subject("Методы оптимизации", 1, 1),
                new Subject("Физика", 1, 1),
                new Subject("ЭГА", 1, 0),
                new Subject("Проектирование ИС", 1, 0),
                new Subject("Высокоуровневые методы программирования", 1, 1),
                new Subject("Математический анализ", 1, 0)
            };
        }

        static List<Group> InitializeGroups(int numGroups, List<Subject> subjects)
        {
            List<Group> groups = new List<Group>();
            for (int i = 0; i < numGroups; i++)
            {
                groups.Add(new Group(i + 1, subjects));
            }
            return groups;
        }
    }
}
