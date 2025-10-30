﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Entities.Academics;
using TN.Shared.Domain.Entities.Attendances;
using TN.Shared.Domain.Entities.FeeAndAccounting;
using TN.Shared.Domain.Primitive;

namespace TN.Shared.Domain.Entities.Students
{
    public class StudentData : Entity
    {

        public StudentData(): base(null)
        {
            
        }

        public StudentData(
            string id, 
            string firstName,
            string? middleName,
            string lastName,
            string admissionNumber,
            GenderStatus gender,
            StudentStatus status,
            DateTime dateOfBirth,
            string? email, 
            string? phoneNumber, 
            string? imageUrl,
            string? address,
            DateTime enrollmentDate, 
            string? parentId,
            string? classSectionId,
            string createdBy, 
            DateTime createdAt, 
            string modifiedBy, 
            DateTime modifiedAt)
            : base(id)
        {
            FirstName = firstName;
            MiddleName = middleName;
            LastName = lastName;
            Gender = gender;
            Status = status;
            AdmissionNumber = admissionNumber;
            DateOfBirth = dateOfBirth;
            Email = email;
            ImageUrl = imageUrl;
            Address = address;
            PhoneNumber = phoneNumber;
            EnrollmentDate = enrollmentDate;
            ParentId = parentId;
            ClassSectionId = classSectionId;
            CreatedBy = createdBy;
            CreatedAt = createdAt;
            ModifiedBy = modifiedBy;
            ModifiedAt = modifiedAt;
            StudentFees = new List<StudentFee>();
            StudentAttendances = new List<StudentAttendance>();
            ExamResults = new List<ExamResult>();
        }

        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string AdmissionNumber { get; set; }
        public string LastName { get; set; }

        public string? ParentId { get; set; }
        public Parent? Parent { get; set; }
        public string ClassSectionId { get; set; }
        public ClassSection ClassSection { get; set; }

        public GenderStatus Gender { get; set; }
        public StudentStatus Status { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? ImageUrl { get; set; }
        public string? Address { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; } = DateTime.Now;
        public ICollection<StudentFee> StudentFees { get; set; }
        public ICollection<StudentAttendance> StudentAttendances { get; set; }
        public ICollection<ExamResult> ExamResults { get; set; }
       



    }

    public enum GenderStatus
    {
        Male,
        Female,
        Others
    }

    public enum StudentStatus
    {
        Active,
        Inactive,
        Graduated,
        Suspended
    }
}
