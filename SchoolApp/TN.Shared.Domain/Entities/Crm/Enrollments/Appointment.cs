using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Entities.Crm.Enrollments;
using TN.Shared.Domain.Entities.Crm.Lead;
using TN.Shared.Domain.Primitive;
using static Azure.Core.HttpHeader;
using static TN.Shared.Domain.Enum.HelperEnum;

namespace TN.Shared.Domain.Entities.Crm.Enrollments
{
    public class Appointment : Entity
    {
        public Appointment(
           
            ): base(null)
        {
            
        }

        public Appointment(
            string id,
            string leadId,
            TimeOnly startTime,
            TimeOnly endTime,
            DateTime appointmentDate,
            string counselorId,
            string notes,
            AppointmentStatus appointmentStatus,
            bool isActive,
            string schoolId,
            string createdBy,
            DateTime createdAt,
            string modifiedBy,
            DateTime modifiedAt
            ) : base(id)
        {
            LeadId = leadId;
            StartTime = startTime;
            EndTime = endTime;
            AppointmentDate = appointmentDate;
            CounselorId = counselorId;
            Notes = notes;
            CreatedBy = createdBy;
            ModifiedBy = modifiedBy;
            CreatedAt = createdAt;
            ModifiedAt = modifiedAt;
            IsActive = isActive;
            SchoolId = schoolId;
            AppointmentStatus = appointmentStatus;


        }

        public void Complete(string notes)
        {
            AppointmentStatus = AppointmentStatus.Completed;
            Notes = notes;
        }

        public void Cancel(string notes)
        {
            AppointmentStatus = AppointmentStatus.Cancelled;
            Notes = notes;
        }

        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public string LeadId { get; set; }
        public CrmLead CrmLead { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string CounselorId { get; set; }
        public Counselor Counselor { get; set; }
        public string Notes { get; set; }
        public AppointmentStatus AppointmentStatus { get; set; }

        public string SchoolId { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;
   
    }
}

//public async Task<List<Appointment>> GetCounselorAppointments(Guid counselorId)
//{
//    return await _context.Appointments
//        .Include(x => x.Lead)
//        .Where(x => x.CounselorId == counselorId)
//        .ToListAsync();
//}

//public async Task CompleteAppointment(Guid appointmentId, string notes)
//{
//    var appointment = await _context.Appointments
//        .FirstOrDefaultAsync(x => x.Id == appointmentId);

//    if (appointment == null)
//        throw new Exception("Appointment not found");

//    appointment.Complete(notes);

//    await _context.SaveChangesAsync();
//}
