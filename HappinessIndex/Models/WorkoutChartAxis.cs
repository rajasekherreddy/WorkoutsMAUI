using System;
using System.Collections.Generic;
using System.Text;

namespace HappinessIndex.Models
{
    public class WorkoutChartAxis
    {
        public long  Id { get; set; }
        public string Name { get; set; }
        public string SubName { get; set; }
        public string Value { get; set; }
        public string PageName { get; set; }

        public DateTime WorkoutDate { get; set; }
        public long Count { get; set; }
    }
}
