using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;

//Это снейк кейс ^-^
namespace lab_work_inclass
{
    
    internal class Program
    {
        internal const int NUMDAYS = 7;
        internal const int NUMPAIRS = 6;
        internal const int MAXPAIRS = 4;
        static void Main(string[] args)
        {
            int numGroups = 3;
            int numSubjects = 10; // todo: Возможно, уже не нужен
            int numLections = 10; // todo: Возможно, уже не нужен
            int numPractics = 5; // todo: Возможно, уже не нужен
            int numLectRooms = 2;
            int numTermRooms = 1;

            List<Lectoriy> lectories = InitializeLectories(numLectRooms);
            List<Terminal> terminals = InitializeTerminals(numTermRooms);
            List<Subject> subjects = InitializeSubjects();
            List<Group> groups = InitializeGroups(numGroups, subjects);

            AllocMilitaryDepartment(groups);
            AllocLecturesAndPractices(groups, lectories, terminals);

            GenerateScheduleReport("testReport.xlsx", groups, lectories, terminals);
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

        static void AllocMilitaryDepartment(List<Group> groups)
        {
            int availableDays = NUMDAYS;

            foreach(var group in groups)
            {
                group.AssignMilitaryDay(availableDays - 1);
                availableDays--;
            }
        }

        static void AllocLecturesAndPractices(List<Group> groups, List<Lectoriy> lectories, List<Terminal> terminals)
        {
            for(int day = 0; day < NUMDAYS; day++)
            {
                // todo: Изменить способ расстановки приоритетов
                Random rnd = new Random();
                List<int> priorities = new List<int>();
                for (int i = 0; i < groups.Count; i++)
                {
                    int priority;
                    do priority = rnd.Next(groups.Count);
                    while (priorities.Contains(priority));

                    priorities.Add(priority);
                }

                for(int pair = 0; pair < NUMPAIRS; pair++)
                {
                    foreach(var indGroupPriority in priorities)
                    {
                        Group currentGroup = groups[indGroupPriority];
                        if (currentGroup.IsMilitaryDay(day) || currentGroup.GetNumPairs(day) >= MAXPAIRS)
                            continue;

                        foreach (var lectory in lectories)
                        {
                            if(lectory.IsAvailable(day, pair))
                            {
                                Subject subjectToAssign = currentGroup.FindLecture();
                                if(subjectToAssign != null)
                                {
                                    currentGroup.AssignLecture(day,pair, subjectToAssign, lectory);
                                    lectory.Employment[day][pair] = true;
                                    break;
                                }
                            }
                        }

                        foreach (var terminal in terminals)
                        {
                            if (terminal.IsAvailable(day, pair))
                            {
                                Subject practiceToAssign = currentGroup.FindPractice();
                                if (practiceToAssign != null)
                                {
                                    currentGroup.AssignPractice(day, pair, practiceToAssign, terminal);
                                    terminal.Employment[day][pair] = true;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        static void GenerateScheduleReport(string path, List<Group> groups, List<Lectoriy> lectories, List<Terminal> terminals)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Расписание");

                worksheet.Cell(1, 1).Value = " ";

                List<string> days = new List<string>() { "Понедельник", "Вторник", "Среда",
                    "Четверг", "Пятница", "Суббота", "Воскресенье" };

                for (int day = 0; day < NUMDAYS; day++)
                {
                    worksheet.Cell(day * NUMPAIRS + 2, 1).Value = days[day];
                    worksheet.Cell(day * NUMPAIRS + 2, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                    worksheet.Range(day * NUMPAIRS + 2, 1, day * NUMPAIRS + NUMPAIRS + 1, 1).Merge();
                    worksheet.Range(day * NUMPAIRS + 2, 1, day * NUMPAIRS + NUMPAIRS + 1, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                    for (int pair = 0; pair < NUMPAIRS; pair++)
                    {
                        worksheet.Cell(day * NUMPAIRS + pair + 2, 2).Value = $"{pair + 1} пара";
                    }
                }

                int numAudience = 1;
                // todo: Оптимизировать код
                foreach(var lectory in lectories)
                {
                    worksheet.Cell(1, numAudience + 2).Value = $"Аудитория {numAudience} (лекторий)";
                    numAudience++;
                }
                foreach (var terminal in terminals)
                {
                    worksheet.Cell(1, numAudience + 2).Value = $"Аудитория {numAudience} (терминал-класс)";
                    numAudience++;
                }
                worksheet.Cell(1, numAudience + 2).Value = "Военная кафедра";

                for(int day = 0; day < NUMDAYS; day++)
                {
                    for(int pair = 0; pair < NUMPAIRS; pair++) 
                    { 
                        foreach(var group in groups)
                        {
                            var lecture = group.GetLecture(day, pair);
                            if (lecture != null)
                            {
                                var assignedLectory = group.GetAssignedLectory(day, pair);
                                if (assignedLectory != -1)
                                {
                                    // Заполняем ячейку для лектория
                                    worksheet.Cell(day * NUMPAIRS + pair + 2, assignedLectory + 2)
                                             .Value = $"Группа {group.Number} {lecture}";
                                }
                            }

                            var practice = group.GetPractice(day, pair);
                            if (practice != null)
                            {
                                var assignedTerminal = group.GetAssignedTerminal(day, pair);
                                if (assignedTerminal != -1)
                                {
                                    // Заполняем ячейку для терминала
                                    worksheet.Cell(day * NUMPAIRS + pair + 2, assignedTerminal + lectories.Count + 2)
                                             .Value = $"Группа {group.Number} {practice}";
                                }
                            }
                        }
                    }
                }

                workbook.SaveAs(path);
            }
        }
    }
}
