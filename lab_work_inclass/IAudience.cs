using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab_work_inclass
{
    internal interface IAudience
    {
        int Number { get; }
        List<List<bool>> Employment { get; }
    }
}
