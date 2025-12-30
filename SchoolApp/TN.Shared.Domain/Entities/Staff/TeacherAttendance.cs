using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Primitive;

namespace TN.Shared.Domain.Entities.Staff
{
    public class TeacherAttendance : Entity
    {
        public TeacherAttendance() : base(null)
        {
            
        }

        public TeacherAttendance(
            string id,
            string uniqueId,
            DateTime attendenceDateAndTime,
            bool isPresent,
            string filePath,
            string academicTeamId
            ): base(id)
        {
            UniqueId = uniqueId;
            AttendanceDateAndTime = attendenceDateAndTime;
            IsPresent = isPresent;
            FilePath = filePath;
            AcademicTeamid = academicTeamId;

            
        }

        public string UniqueId { get; set; }
        public DateTime AttendanceDateAndTime { get; set; }
        public bool IsPresent { get; set; }
        public string FilePath { get; set; }
        public string AcademicTeamid { get; set; }
    }
}
