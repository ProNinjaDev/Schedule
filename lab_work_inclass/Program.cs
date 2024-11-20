using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;


namespace lab_work_inclass
{
    
    internal class Program
    {
        internal const int NUMDAYS = 6;
        internal const int NUMPAIRS = 6;
        internal const int MAXPAIRS = 4;
        static void Main(string[] args)
        {
            int numGroups = 3;
            int numLectRooms = 2;
            int numTermRooms = 1;

            Schedule schedule = new Schedule(numGroups, numLectRooms, numTermRooms);

            schedule.AllocateSchedule();
            schedule.GenerateReport("testReport.xlsx");
        }

    }
}
