using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Entities.Students;
using TN.Shared.Domain.Primitive;

namespace TN.Shared.Domain.Entities.Attendances
{
    public class StudentAttendance: Entity
    {
        public StudentAttendance(
            ): base(null)
        {
            
        }

        public StudentAttendance(
            string id,
            string studentId,
            DateTime attendanceDate,
            AttendanceStatus attendanceStatus,
            string teacherId,
            string? remarks,
            string createdBy,
            DateTime createdAt,
            string modifiedBy,
            DateTime modifiedAt
            ) : base(id)
        {
            StudentId = studentId;
            AttendanceDate = attendanceDate;
            AttendanceStatus = attendanceStatus;
            TeacherId = teacherId;
            Remarks = remarks;
            CreatedBy = createdBy;
            CreatedAt = createdAt;
            ModifiedBy = modifiedBy;
            ModifiedAt = modifiedAt;

        }
        public string StudentId { get; set; }

        public StudentData Student { get; set; }
        public DateTime AttendanceDate { get; set; }
        public AttendanceStatus AttendanceStatus { get; set; }
        public string TeacherId { get; set; }
        public Teacher Teacher { get; set; }
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
