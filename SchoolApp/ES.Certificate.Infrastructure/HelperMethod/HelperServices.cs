using AutoMapper;
using ES.Academics.Application.Academics.Queries.MarkSheetByStudent;
using ES.Certificate.Application.Certificates.Queries.CertificateTemplateById;
using ES.Certificate.Application.ServiceInterface.IHelperMethod;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TN.Account.Domain.Entities;
using TN.Authentication.Domain.Entities;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.Entities.Certificates;
using TN.Shared.Domain.Entities.OrganizationSetUp;
using TN.Shared.Domain.Entities.Students;
using TN.Shared.Domain.IRepository;

namespace ES.Certificate.Infrastructure.HelperMethod
{
    public class HelperServices : IHelperMethodServices
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCacheRepository _memoryCacheRepository;
        private readonly ITokenService _tokenService;
        private readonly IGetUserScopedData _getUserScopedData;
        private readonly IDateConvertHelper _dateConverter;
        private readonly FiscalContext _fiscalContext;

        public HelperServices(IDateConvertHelper dateConverter, IGetUserScopedData getUserScopedData, FiscalContext fiscalContext, ITokenService tokenService, IUnitOfWork unitOfWork, IMemoryCacheRepository memoryCacheRepository, IMapper mapper)
        {
            _dateConverter = dateConverter;
            _getUserScopedData = getUserScopedData;
            _tokenService = tokenService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _memoryCacheRepository = memoryCacheRepository;
            _fiscalContext = fiscalContext;
        }

        public async Task<string> CalculateGPA(MarksSheetDTOs marksSheetDTOs)
        {
            try
            {

                var studentsDetails = await _unitOfWork.BaseRepository<StudentData>()
       .GetSingleWithProjectionAsync(
           projection: s => new StudentDetailsDto
           {
               Id = s.Id,
               FirstName = s.FirstName,
               LastName = s.LastName,
               Email = s.Email,
               EnrollmentDate = s.EnrollmentDate,
               ExamResults = s.ExamResults
                   .Where(er => er.IsActive && er.StudentId == marksSheetDTOs.studentId && er.ExamId == marksSheetDTOs.examId)
                   .OrderByDescending(er => er.CreatedAt)
                   .Select(er => new ExamResultDto
                   {
                       Id = er.Id,
                       MarksObtained = er.MarksOtaineds.Sum(x=>x.MarksObtaineds),


                       Exam = new ExamDto
                       {
                           Id = er.Exam.Id,
                           Name = er.Exam.Name,
                           TotalMarks = er.Exam.TotalMarks,

                       },

                       //SubjectDto = new SubjectDto
                       //{
                       //    Id = er.MarksObtained,
                       //    Name = er.Subject.Name,
                       //    CreditHour = er.MarksOtaineds.FirstOrDefault().Subject.CreditHour,
                       //}

                   })
                   .ToList(),
               IssuedCertificates = s.IssuedCertificates
                   .Where(ic => ic.IsActive)
                   .OrderByDescending(ic => ic.IssuedDate)
                   .Select(ic => new IssuedCertificateDto
                   {
                       Id = ic.Id,
                       CertificateNumber = ic.CertificateNumber,
                       IssuedDate = ic.IssuedDate,

                   })
                   .ToList()
           },
           // Predicate
           predicate: x => x.Id == marksSheetDTOs.studentId,
           queryModifier: query => query
               .Include(s => s.ExamResults.Where(er => er.IsActive))
                   .ThenInclude(er => er.Exam)
               .Include(s => s.IssuedCertificates.Where(ic => ic.IsActive))
       );





                if (studentsDetails == null || studentsDetails.ExamResults == null || !studentsDetails.ExamResults.Any())
                    return "0.00";

                // Assuming each exam has TotalMarks and ExamResult has MarksObtained
                decimal totalWeightedGPA = 0;
                decimal totalCredits = 0;

                foreach (var result in studentsDetails.ExamResults)
                {
                    var exam = result.Exam;
                    var subject = result.SubjectDto;

                    if (exam == null || exam.TotalMarks <= 0 || subject == null)
                        continue;

                    // Calculate percentage
                    decimal percentage = (result.MarksObtained / exam.TotalMarks) * 100m;

                    // Convert to grade point
                    decimal gradePoint = GetGradePointFromPercentage(percentage);

                    // Use Subject's credit hours
                    decimal credit = subject.CreditHour.HasValue && subject.CreditHour.Value > 0
                     ? subject.CreditHour.Value
                     : 1m;



                    totalWeightedGPA += gradePoint * credit;
                    totalCredits += credit;
                }

                decimal gpa = totalCredits > 0 ? totalWeightedGPA / totalCredits : 0m;
                return Math.Round(gpa, 2).ToString("0.00");
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while calculating GPA.", ex);
            }
        }



        private decimal GetGradePointFromPercentage(decimal percentage)
        {
            if (percentage >= 90) return 4.0m;
            if (percentage >= 80) return 3.6m;
            if (percentage >= 70) return 3.2m;
            if (percentage >= 60) return 2.8m;
            if (percentage >= 50) return 2.4m;
            if (percentage >= 40) return 2.0m;
            return 0.0m; // Fail
        }


       

        private string GetDivisionFromPercentage(decimal percentage)
        {
            if (percentage < 0 || percentage > 100)
                throw new ArgumentOutOfRangeException(nameof(percentage),
                    $"Invalid percentage value: {percentage}. Expected range 0–100.");

            if (percentage >= 90) return "Distinction";
            if (percentage >= 80) return "First Division";
            if (percentage >= 70) return "Second Division";
            if (percentage >= 60) return "Third Division";
            if (percentage >= 50) return "Third Division";
            if (percentage >= 40) return "Pass";
            return "Fail";
        }



        public class StudentDetailsDto
        {
            public string Id { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public DateTime EnrollmentDate { get; set; }
            public List<ExamResultDto> ExamResults { get; set; } = new();
            public List<IssuedCertificateDto> IssuedCertificates { get; set; } = new();
        }

        public class ExamResultDto
        {
            public string Id { get; set; }

            public decimal MarksObtained { get; set; }

            public SubjectDto SubjectDto { get; set;  }
   
            public ExamDto Exam { get; set; }
        }

        public class SubjectDto
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public int? CreditHour { get; set; }
        }

        public class ExamDto
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public decimal TotalMarks { get; set; }
            public string Code { get; set; }
        }

        public class IssuedCertificateDto
        {
            public string Id { get; set; }
            public string CertificateNumber { get; set; }
            public DateTime IssuedDate { get; set; }
            public string CertificateType { get; set; }
            public DateTime? ExpiryDate { get; set; }
        }


        public async Task<string> CalculatePercentage(MarksSheetDTOs marksSheetDTOs)
        {
            try
            {

                var studentsDetails = await _unitOfWork.BaseRepository<StudentData>()
               .GetSingleWithProjectionAsync(
                   projection: s => new StudentDetailsDto
                   {
                       Id = s.Id,
                       FirstName = s.FirstName,
                       LastName = s.LastName,
                       Email = s.Email,
                       EnrollmentDate = s.EnrollmentDate,
                       ExamResults = s.ExamResults
                           .Where(er => er.IsActive && er.StudentId == marksSheetDTOs.studentId && er.ExamId == marksSheetDTOs.examId)
                           .OrderByDescending(er => er.CreatedAt)
                           .Select(er => new ExamResultDto
                           {
                               Id = er.Id,
                               MarksObtained = er.MarksOtaineds.Sum(x=>x.MarksObtaineds),


                               Exam = new ExamDto
                               {
                                   Id = er.Exam.Id,
                                   Name = er.Exam.Name,
                                   TotalMarks = er.Exam.TotalMarks,
             
                               }
                           })
                           .ToList(),
                       IssuedCertificates = s.IssuedCertificates
                           .Where(ic => ic.IsActive)
                           .OrderByDescending(ic => ic.IssuedDate)
                           .Select(ic => new IssuedCertificateDto
                           {
                               Id = ic.Id,
                               CertificateNumber = ic.CertificateNumber,
                               IssuedDate = ic.IssuedDate,
                    
                           })
                           .ToList()
                   },
                   // Predicate
                   predicate: x => x.Id == marksSheetDTOs.studentId,
                   queryModifier: query => query
                       .Include(s => s.ExamResults.Where(er => er.IsActive))
                           .ThenInclude(er => er.Exam)
                       .Include(s => s.IssuedCertificates.Where(ic => ic.IsActive))
               );




                if (studentsDetails == null)
                    return "";

                decimal totalMarks = studentsDetails.ExamResults.Sum(er => er.Exam?.TotalMarks ?? 0);

                decimal obtainedMarks = studentsDetails.ExamResults.Sum(er => er.MarksObtained);
                decimal percentage = totalMarks > 0 ? (obtainedMarks * 100m / totalMarks) : 0m;

                return Math.Round(percentage, 2) + "%";

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching Certificate Template by using Id", ex);
            }

        }

        public async Task<string> CalculateDivision(MarksSheetDTOs marksSheetDTOs)
        {

            try
            {
                var percentageString = await CalculatePercentage(marksSheetDTOs);


                // Remove % and trim spaces safely
                var cleanPercentageString = percentageString
                    ?.Replace("%", "")
                    ?.Trim();


                if (string.IsNullOrWhiteSpace(percentageString))
                    throw new InvalidOperationException("Percentage value is empty or null.");

                if (!decimal.TryParse(cleanPercentageString, out var percentage))
                    throw new FormatException($"Invalid percentage format: '{percentageString}'");

                var division =  GetDivisionFromPercentage(percentage);

                return division.ToString();
            }
            catch (Exception ex)
            {
                // Preserve original error for clarity
                throw new Exception($"Error while calculating division for student ID: {marksSheetDTOs.studentId}. Details: {ex.Message}", ex);
            }
        }
    }
}
