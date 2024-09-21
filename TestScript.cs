using System;
using System.Collections.Generic;
using System.Linq;
using static TimePeriod;
class TestScript
{
    public static (string,bool) RunScript( )
    {
        // Example entry and exit dates
        List<DateTime> entryDates = new List<DateTime>
        {
            new DateTime(2023, 1, 1),
            // new DateTime(2023, 4, 1),
            // new DateTime(2023, 4, 19),
            // Add more entry dates as needed
        };

        List<DateTime> exitDates = new List<DateTime>
        {
            new DateTime(2023, 6, 30),
            // new DateTime(2023, 4, 10),
            // new DateTime(2023, 4, 22),
            // Add corresponding exit dates
        };
        

        return TimePeriod.CalculateRemainingDays(entryDates, exitDates);
    }
}
