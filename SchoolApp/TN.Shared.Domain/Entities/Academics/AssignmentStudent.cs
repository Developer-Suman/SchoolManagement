using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Authentication.Domain.Entities;
using TN.Shared.Domain.Entities.Students;
using TN.Shared.Domain.Primitive;

namespace TN.Shared.Domain.Entities.Academics
{
    public class AssignmentStudent: Entity
    {
        public AssignmentStudent(
            string id,
            string assignmentId,
            string studentId,
            bool isSubmitted,
            DateTime? submittedAt,
            string? submissionText,
            string? submissionFileUrl,
            decimal? marks,
            string? teacherRemarks,
             bool isActive,
            string schoolId,
            string createdBy,
            DateTime createdAt,
            string modifiedBy,
            DateTime? modifiedAt
            ) : base(null)
        {
            AssignmentId = assignmentId;
            StudentId = studentId;
            IsSubmitted = isSubmitted;
            SubmittedAt = submittedAt;
            Marks = marks;
            SubmissionText = submissionText;
            SubmissionFileUrl = submissionFileUrl;
            TeacherRemarks = teacherRemarks;
            IsActive = isActive;
            SchoolId = schoolId;
            CreatedBy = createdBy;
            CreatedAt = createdAt;
            ModifiedBy = modifiedBy;
            ModifiedAt = modifiedAt;

        }

        public string? SubmissionText { get; set; }
        public string? SubmissionFileUrl { get; set; }
        public string? TeacherRemarks { get; set; }
        public string AssignmentId { get; set; }
        public Assignment Assignment { get; set; }

        public string StudentId { get; set; }
        public StudentData Student { get; set; }

        public bool IsSubmitted { get; set; }
        public DateTime? SubmittedAt { get; set; }
        public decimal? Marks { get; set; }
        public bool IsActive { get; set; }

        public string SchoolId { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
