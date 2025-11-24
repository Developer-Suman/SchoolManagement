using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Entities.Students;
using TN.Shared.Domain.Static.LowerCase;

namespace ES.Student.Application.Student.Command.AddStudents
{
    public class AddStudentsRequest
    {
        public string firstName { get; set; }
        public string? middleName { get; set; }
        public string lastName { get; set; }
        public string registrationNumber { get; set; }
        public GenderStatus genderStatus { get; set; }
        public StudentStatus studentStatus { get; set; }
        public DateTime dateOfBirth { get; set; }
        public string? email { get; set; }
        public string? phoneNumber { get; set; }
        public string? address { get; set; }
        public DateTime enrollmentDate { get; set; }
        public string? parentId { get; set; }
        public string? classSectionId { get; set; }
        public int provinceId { get; set; }
        public int districtId { get; set; }
        public int wardNumber { get; set; }
        public int? vdcId { get; set; }
        public int? municipalityId { get; set; }
        public IFormFile? studentImg { get; set; }
        public string? classId { get; set; }

        public AddStudentsRequest() { }

        public AddStudentsRequest(
            string firstName,
            string? middleName,
            string lastName,
            string registrationNumber,
            GenderStatus genderStatus,
            StudentStatus studentStatus,
            DateTime dateOfBirth,
            string? email,
            string? phoneNumber,
            string? address,
            DateTime enrollmentDate,
            string? parentId,
            string? classSectionId,
            int provinceId,
            int districtId,
            int wardNumber,
            int? vdcId,
            int? municipalityId,
            IFormFile? studentImg,
            string? classId)
        {
            this.firstName = firstName;
            this.middleName = middleName;
            this.lastName = lastName;
            this.registrationNumber = registrationNumber;
            this.genderStatus = genderStatus;
            this.studentStatus = studentStatus;
            this.dateOfBirth = dateOfBirth;
            this.email = email;
            this.phoneNumber = phoneNumber;
            this.address = address;
            this.enrollmentDate = enrollmentDate;
            this.parentId = parentId;
            this.classSectionId = classSectionId;
            this.provinceId = provinceId;
            this.districtId = districtId;
            this.wardNumber = wardNumber;
            this.vdcId = vdcId;
            this.municipalityId = municipalityId;
            this.studentImg = studentImg;
            this.classId = classId;
        }
    }
}
