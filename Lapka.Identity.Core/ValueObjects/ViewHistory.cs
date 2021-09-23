﻿namespace Lapka.Identity.Core.ValueObjects
{
    public class ViewHistory
    {
        public int MonthOfTheYear { get; }
        public int Year { get; }
        public int Views { get; }

        public ViewHistory(int monthOfTheYear, int year, int views)
        {
            MonthOfTheYear = monthOfTheYear;
            Year = year;
            Views = views;
        }
    }
}