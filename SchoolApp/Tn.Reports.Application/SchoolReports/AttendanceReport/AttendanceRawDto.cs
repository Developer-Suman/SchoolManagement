using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Reports.Application.SchoolReports.AttendanceReport
{
    public class AttendanceRawDto
    {
        public string StudentId { get; set; }
        public string AcademicTeamId { get; set; }  
        public string ClassId { get; set; }        
        public string AttendanceDateNepali { get; set; }
        public int AttendanceStatus { get; set; }
        public string Remarks { get; set; }
        public DateTime? CreatedAt { get; set; }   
    }
}
