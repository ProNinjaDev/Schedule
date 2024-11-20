using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab_work_inclass
{
    internal class Schedule
    {
        private List<Lectoriy> Lectories;
        private List<Terminal> Terminals;
        private List<Group> Groups;

        public Schedule(int numGroups, int numLectRooms, int numTermRooms) 
        {
            Lectories = InitializeLectories(numLectRooms);
            Terminals = InitializeTerminals(numTermRooms);
            Groups = InitializeGroups(numGroups, InitializeSubjects());
        }

        private List<Lectoriy> InitializeLectories(int numLectRooms)
        {
            List<Lectoriy> lectories = new List<Lectoriy>();
            for (int i = 0; i < numLectRooms; i++)
            {
                lectories.Add(new Lectoriy(i + 1));
            }
            return lectories;
        }

        private List<Terminal> InitializeTerminals(int numTermRooms)
        {
            List<Terminal> terminals = new List<Terminal>();
            for (int i = 0; i < numTermRooms; i++)
            {
                terminals.Add(new Terminal(i + 1));
            }
            return terminals;
        }

        private List<Subject> InitializeSubjects()
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

        private List<Group> InitializeGroups(int numGroups, List<Subject> subjects)
        {
            List<Group> groups = new List<Group>();
            for (int i = 0; i < numGroups; i++)
            {
                if (i % 3 == 0)
                    groups.Add(new RegularGroup(i + 1, subjects));
                else if (i % 3 == 1)
                    groups.Add(new LectoralGroup(i + 1, subjects));
                else
                    groups.Add(new PracticalGroup(i + 1, subjects));
            }
            return groups;
        }

        private void AllocMilitaryDepartment(List<Group> groups)
        {
            int availableDays = Program.NUMDAYS;

            foreach (var group in groups)
            {
                group.AssignMilitaryDay(availableDays - 1);
                availableDays--;
            }
        }

        private void AllocLecturesAndPractices(List<Group> groups, List<Lectoriy> lectories, List<Terminal> terminals)
        {
            for (int day = 0; day < Program.NUMDAYS; day++)
            {
                // todo: Изменить способ расстановки приоритетов
                List<int> priorities = GenerateGroupPriorities(groups.Count);

                for (int pair = 0; pair < Program.NUMPAIRS; pair++)
                {
                    foreach (var indGroupPriority in priorities)
                    {
                        Group currentGroup = groups[indGroupPriority];
                        if (currentGroup.IsMilitaryDay(day) || currentGroup.GetNumPairs(day) >= Program.MAXPAIRS)
                            continue;

                        foreach (var lectory in lectories)
                        {
                            if (lectory.IsAvailable(day, pair))
                            {
                                Subject subjectToAssign = currentGroup.FindLecture();
                                if (subjectToAssign != null)
                                {
                                    currentGroup.AssignLecture(day, pair, subjectToAssign, lectory);
                                    lectory.Employment[day][pair] = true;
                                    break;
                                }
                            }
                        }

                        foreach (var terminal in terminals)
                        {
                            if (terminal.IsAvailable(day, pair) && currentGroup.AssignedLectories[day][pair] == -1)
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

        private void GenerateScheduleReport(string path, List<Group> groups, List<Lectoriy> lectories, List<Terminal> terminals)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Расписание");

                worksheet.Cell(1, 1).Value = " ";

                List<string> days = new List<string>() { "Понедельник", "Вторник", "Среда",
                    "Четверг", "Пятница", "Суббота", "Воскресенье" };

                for (int day = 0; day < Program.NUMDAYS; day++)
                {
                    worksheet.Cell(day * Program.NUMPAIRS + 2, 1).Value = days[day];
                    worksheet.Cell(day * Program.NUMPAIRS + 2, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                    worksheet.Range(day * Program.NUMPAIRS + 2, 1, day * Program.NUMPAIRS + Program.NUMPAIRS + 1, 1).Merge();
                    worksheet.Range(day * Program.NUMPAIRS + 2, 1, day * Program.NUMPAIRS + Program.NUMPAIRS + 1, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                    for (int pair = 0; pair < Program.NUMPAIRS; pair++)
                    {
                        worksheet.Cell(day * Program.NUMPAIRS + pair + 2, 2).Value = $"{pair + 1} пара";
                    }
                }

                int numAudience = 1;

                foreach (var lectory in lectories)
                {
                    worksheet.Cell(1, numAudience + 2).Value = $"Аудитория {numAudience}\n(лекторий)";
                    numAudience++;
                }
                foreach (var terminal in terminals)
                {
                    worksheet.Cell(1, numAudience + 2).Value = $"Аудитория {numAudience}\n(терминал-класс)";
                    numAudience++;
                }
                worksheet.Cell(1, numAudience + 2).Value = "Военная кафедра";

                for (int day = 0; day < Program.NUMDAYS; day++)
                {
                    for (int pair = 0; pair < Program.NUMPAIRS; pair++)
                    {
                        foreach (var group in groups)
                        {
                            var lecture = group.GetLecture(day, pair);
                            if (lecture != null)
                            {
                                var assignedLectory = group.GetAssignedLectory(day, pair);
                                if (assignedLectory != -1)
                                {
                                    worksheet.Cell(day * Program.NUMPAIRS + pair + 2, assignedLectory + 2)
                                             .Value = $"Группа {group.Number}\n{lecture}";
                                }
                            }

                            var practice = group.GetPractice(day, pair);
                            if (practice != null)
                            {
                                var assignedTerminal = group.GetAssignedTerminal(day, pair);
                                if (assignedTerminal != -1)
                                {
                                    worksheet.Cell(day * Program.NUMPAIRS + pair + 2, assignedTerminal + lectories.Count + 2)
                                             .Value = $"Группа {group.Number}\n{practice}";
                                }
                            }
                        }
                    }

                    foreach (var group in groups)
                    {
                        if (group.IsMilitaryDay(day))
                        {
                            worksheet.Range(day * Program.NUMPAIRS + 2, numAudience + 2, day * Program.NUMPAIRS + Program.NUMPAIRS + 1, numAudience + 2).Merge();
                            worksheet.Cell(day * Program.NUMPAIRS + 2, numAudience + 2).Value = $"Группа {group.Number}";
                            worksheet.Range(day * Program.NUMPAIRS + 2, numAudience + 2, day * Program.NUMPAIRS + Program.NUMPAIRS + 1, numAudience + 2).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                            worksheet.Range(day * Program.NUMPAIRS + 2, numAudience + 2, day * Program.NUMPAIRS + Program.NUMPAIRS + 1, numAudience + 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        }
                    }

                    for (int i = 1; i < numAudience + 4; i++)
                    {
                        worksheet.Column(i).AdjustToContents();
                    }
                    worksheet.Column(1).Width = 20;
                    worksheet.Column(2).Width = 7;

                    for (int i = 1; i < Program.NUMDAYS * Program.NUMPAIRS + 2; i++)
                    {
                        worksheet.Row(i).Height = 40;
                    }

                    worksheet.Cells().Style.Alignment.WrapText = true;
                    worksheet.Cells().Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Cells().Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                }

                workbook.SaveAs(path);
            }
        }

        private List<int> GenerateGroupPriorities(int numGroups)
        {
            Random rnd = new Random();
            List<int> priorities = new List<int>();

            for (int i = 0; i < numGroups; i++)
            {
                int priority;
                do priority = rnd.Next(numGroups);
                while (priorities.Contains(priority));

                priorities.Add(priority);
            }

            return priorities;
        }

        public void AllocateSchedule()
        {
            AllocMilitaryDepartment(Groups);
            AllocLecturesAndPractices(Groups, Lectories, Terminals);
        }

        public void GenerateReport(string path)
        {
            GenerateScheduleReport(path, Groups, Lectories, Terminals);
        }
    }
}
