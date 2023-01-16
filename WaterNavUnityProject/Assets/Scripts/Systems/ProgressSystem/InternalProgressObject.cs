using System;
using DataStructures;
using UnityEngine.UIElements;

namespace RCR.Systems.ProgressSystem
{
    public class InternalProgressObject
    {
        public UnityDateTime DateTime;
        public string Name;
        public string Description;
        public int ParentID;
        private ProgressBar Bar;
        public InternalProgressObject(string name,string description,ProgressBar bar, int parentID = -1)
        {
            DateTime = new UnityDateTime();
            Description = description;
            ParentID = parentID;
            Name = name;
            Bar = bar;
        }

        public void Report(float progress)
        {
            TimeSpan ts = TimeSpan.FromSeconds(progress);
            DateTime.Years = ts.Days / 365;
            DateTime.Months = (ts.Days - (ts.Days / 365) * 365) / 30;
            DateTime.Days = (ts.Days - (ts.Days / 365) * 365) - ((ts.Days - (ts.Days / 365) * 365) / 30) * 30;
            DateTime.Hours = ts.Hours;
            DateTime.Minutes = ts.Minutes;
            DateTime.Seconds = ts.Seconds;
        }
        public void Report(double progress)
        {
            TimeSpan ts = TimeSpan.FromSeconds(progress);
            DateTime.Years = ts.Days / 365;
            DateTime.Months = (ts.Days - (ts.Days / 365) * 365) / 30;
            DateTime.Days = (ts.Days - (ts.Days / 365) * 365) - ((ts.Days - (ts.Days / 365) * 365) / 30) * 30;
            DateTime.Hours = ts.Hours;
            DateTime.Minutes = ts.Minutes;
            DateTime.Seconds = ts.Seconds;
        }
        public void Report(float progress, string description)
        {
            Description = description;
            Report(progress);
        }
        public void Report(double progress, string description)
        {
            Description = description;
            Report(progress);
        }
    }
}