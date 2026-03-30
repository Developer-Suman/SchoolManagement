using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Reports.Application.SchoolReports.AttendanceReport
{
    public class AttendanceMonthlyRawDto
    {
        public string StudentId { get; set; }
        public string MonthKey { get; set; }
        public int PresentCount { get; set; }
        public int AbsentCount { get; set; }
        public int ExcusedCount { get; set; }
        public int LateCount { get; set; }
        public int LeftEarlyCount { get; set; }
    }
}
