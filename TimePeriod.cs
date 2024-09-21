using System;
using System.Collections.Generic;
using System.Linq;

class TimePeriod
{
    public DateTime StartDate { get; }
    public DateTime EndDate { get; }

    public TimePeriod(DateTime start, DateTime end)
    {
        StartDate = start;
        EndDate = end;
    }

    public int GetDuration()
    {
        TimeSpan duration = EndDate - StartDate;
        return duration.Days + 1; // Adding 1 to include both start and end dates
    }
    public bool DayInTimePeriod(DateTime day)
    {
        return day <= StartDate;
    }

    //return true if day is in the last 180 days window
    public bool RelevantPeriod()
    {
        return DayInTimePeriod(DateTime.Today.AddDays(-180));
    }

    public override String ToString()
    {
        return $"start-date:{StartDate}, end-date:{EndDate}";
    }
    public static (string, bool) CalculateRemainingDays(
        List<DateTime> entryDates, List<DateTime> exitDates
        )
    {
        // calculate current date
        // take 180 day window from the MostRecent(currentDay, lastexitday)
        string result = "";
        bool ans = true;
        DateTime lastDate = exitDates[entryDates.Count - 1];
        DateTime currentDate = DateTime.Today;
        int totalRemainingDays = 90;

        DateTime lastExitDate = exitDates.Max();
        // After that I need to calculate how many days were filled out in the last 180

        List<TimePeriod> relevantTimePeriods = new List<TimePeriod>();

        for (int i = 0; i < entryDates.Count; i++)
        {
            DateTime entryDate = entryDates[i];
            DateTime exitDate = exitDates[i];

            TimePeriod timePeriod = new TimePeriod(entryDate, exitDate);
            relevantTimePeriods.Add(timePeriod);
        }
        relevantTimePeriods = relevantTimePeriods.Where(p => p.DayInTimePeriod(lastExitDate.AddDays(-180))).ToList();

        //and for every visit I should say when it will be renewed and one can visit again
        // remaining
        // remaining + first batch renewed
        // remaining + 2nd batch renewed
        //...
        //remaining + last batch

        foreach (var timePeriod in relevantTimePeriods)
        {
            int daysInTimePeriod = (int)(timePeriod.EndDate - timePeriod.StartDate).TotalDays;
            totalRemainingDays -= daysInTimePeriod + 1;
            if (totalRemainingDays <= 0)
            {
                result += "You have used up your 90 days in the last 180-day period.\n";
                ans = false;
                break;
            }
        }
        DateTime remainingDate = lastExitDate.AddDays(totalRemainingDays);
        result += $"You can stay for {totalRemainingDays} more days until {remainingDate.ToShortDateString()}.\n";

        int stayFor = totalRemainingDays;
        foreach (var timePeriod in relevantTimePeriods)
        {
            DateTime entryDate = timePeriod.EndDate.AddDays(91);
            stayFor += (int)(timePeriod.EndDate - timePeriod.StartDate).TotalDays + 1;
            result += $"If you re-enter the Schengen area on {entryDate}, you can stay {stayFor} days until {entryDate.AddDays(stayFor)}.\n";
        }
        return (result,ans);

    }
    //TODO: change this to output separate information that can be used in messages
    //calculate remaining days from a designated end date
}
