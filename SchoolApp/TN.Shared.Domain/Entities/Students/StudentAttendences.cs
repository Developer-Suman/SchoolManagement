using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Entities.Staff;
using TN.Shared.Domain.Primitive;

namespace TN.Shared.Domain.Entities.Students
{
    public class StudentAttendances : Entity
    {
        public StudentAttendances(
            ) : base(null)
        {

        }

        public StudentAttendances(
            string id,
            string studentId,
            DateTime attendanceDate,
            AttendanceStatus attendanceStatus,
            string academicTeamId,
            string? remarks,
            string createdBy,
            DateTime createdAt,
            string modifiedBy,
            DateTime modifiedAt,
            string schoolId,
            bool isActive
            ) : base(id)
        {
            StudentId = studentId;
            AttendanceDate = attendanceDate;
            AttendanceStatus = attendanceStatus;
            AcademicTeamId = academicTeamId;
            Remarks = remarks;
            CreatedBy = createdBy;
            CreatedAt = createdAt;
            ModifiedBy = modifiedBy;
            ModifiedAt = modifiedAt;
            SchoolId = schoolId;
            IsActive = isActive;

        }

        public string SchoolId { get; set; }
        public bool IsActive { get; set; }
        public string StudentId { get; set; }

        public StudentData Student { get; set; }
        public DateTime AttendanceDate { get; set; }
        public AttendanceStatus AttendanceStatus { get; set; }
        public string AcademicTeamId { get; set; }
        public AcademicTeam AcademicTeams { get; set; }
        public string? Remarks { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; } = DateTime.Now;


    }

    public enum AttendanceStatus
    {
        Present,
        Absent,
        Excused,
        Late,
        LeftEarly
    }
}
