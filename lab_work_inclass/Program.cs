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
            int numGroups, numSubjects, numLections, numPractics, numRemainingsPairs, numLectRooms, numTermRooms;
            numGroups = 3;
            numSubjects = 10;
            numLections = 10;
            numPractics = 5;
            numRemainingsPairs = 4;
            numLectRooms = 2;
            numTermRooms = 1;

            List<Lectoriy> lectories = new List<Lectoriy>();

            for (int i = 0; i < numLectRooms; i++)
            {
                lectories.Add(new Lectoriy(i));
            }

            List<Terminal> terminals = new List<Terminal>();
            for (int i = 0; i < numTermRooms; i++)
            {
                terminals.Add(new Terminal(i));
            }

            

        }
    }
}
