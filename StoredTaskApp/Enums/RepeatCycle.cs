using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoredTaskApp.Enums
{
    public enum RepeatCycle
    {
        Daily = 1,
        Weekly = 7,
        Biweekly = 14,
        Monthly = 30,
        BiMonthly = 60,
        Quarterly = 120,
        Biannually = 180,
        Annually = 365
    }
}
