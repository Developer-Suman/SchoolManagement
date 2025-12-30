
using TN.Account.Domain.Entities;
using TN.Authentication.Domain.Entities;
using TN.Shared.Domain.Entities.Academics;
using TN.Shared.Domain.Entities.Certificates;
using TN.Shared.Domain.Entities.Finance;
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
            string registrationNumber,
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
            int provinceId,
            int districtId,
            int wardNumber,
            string createdBy, 
            DateTime createdAt, 
            string modifiedBy, 

            DateTime modifiedAt,
            string schoolId,
            bool isActive,
            int? vdcid,
            int? municipalityId,
            string? classId,
            string? userId

            )
            : base(id)
        {
            FirstName = firstName;
            MiddleName = middleName;
            LastName = lastName;
            Gender = gender;
            Status = status;
            RegistrationNumber = registrationNumber;
            DateOfBirth = dateOfBirth;
            Email = email;
            ImageUrl = imageUrl;
            Address = address;
            PhoneNumber = phoneNumber;
            EnrollmentDate = enrollmentDate;
            ParentId = parentId;
            ClassSectionId = classSectionId;
            ProvinceId = provinceId;
            DistrictId = districtId;
            WardNumber = wardNumber;
            CreatedBy = createdBy;
            CreatedAt = createdAt;
            ModifiedBy = modifiedBy;
            ModifiedAt = modifiedAt;
            SchoolId = schoolId;
            IsActive = isActive;
            VdcId = vdcid;
            ClassId = classId;
            MunicipalityId = municipalityId;
            StudentFees = new List<StudentFee>();
            StudentAttendances = new List<StudentAttendances>();
            ExamResults = new List<ExamResult>();
            IssuedCertificates = new List<IssuedCertificate>();
            SeatAssignments = new List<SeatAssignment>();
            AssignmentStudents= new List<AssignmentStudent>();

        }


        public string? UserId { get; set; }
        public ApplicationUser? Users { get; set; }
        public string SchoolId { get; set; }
        public bool IsActive { get; set; }
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string RegistrationNumber { get; set; }
        public string LastName { get; set; }

        public string? ParentId { get; set; }
        public Parent? Parent { get; set; }

        public string? ClassId { get; set; }
        public Class? Class { get; set; }
        public string? ClassSectionId { get; set; }
        public ClassSection? ClassSection { get; set; }

        public GenderStatus Gender { get; set; }
        public StudentStatus Status { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? ImageUrl { get; set; }
        public string? Address { get; set; }

        public int ProvinceId { get; set; }
        public Province Province { get; set; }
        public int DistrictId { get; set; }
        public District District { get; set; }

        public int? MunicipalityId { get; set; }
        public Municipality? Municipality { get; set; }

        public int? VdcId { get; set; }
        public Vdc? Vdc { get; set; }
        public int WardNumber { get; set;  }



        public Ledger? Ledger { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; } = DateTime.Now;
        public ICollection<StudentFee> StudentFees { get; set; }
        public ICollection<StudentAttendances> StudentAttendances { get; set; }
        public ICollection<ExamResult> ExamResults { get; set; }
        public ICollection<IssuedCertificate> IssuedCertificates { get; set; }
        public ICollection<SeatAssignment> SeatAssignments { get; set; }
        public ICollection<AssignmentStudent> AssignmentStudents { get; set; }

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
