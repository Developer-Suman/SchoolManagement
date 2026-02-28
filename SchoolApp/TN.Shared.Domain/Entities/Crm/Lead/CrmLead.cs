using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Entities.Crm.Profile;
using TN.Shared.Domain.Primitive;
using static TN.Shared.Domain.Enum.EducationLevelEnum;
using static TN.Shared.Domain.Enum.GenderEnum;
using static TN.Shared.Domain.Enum.HelperEnum;

namespace TN.Shared.Domain.Entities.Crm.Lead
{
    public class CrmLead : Entity
    {
        public CrmLead(
            ): base(null)
        {
            
        }

        public CrmLead(
            string id,
            DateTime dateOfBirth,
            Gender gender,
            string contactNumber,
            string permanentAddress,
            EducationLevel educationLevel,
            string completionYear,
            string currentGpa,
            string previousAcademicQualification,

            EnglishProficiency? englishProficiency,
            decimal? bandScore,
            string? languageRemarks,

            string? skillOrTrainingName,
            string? institutionName,
            string? trainingRemarks,
            string? trainingStartDate,
            string? trainingEndDate,


            string source,
            string feedBackOrSuggestion,
            bool isActive,
            string schoolId,
            string createdBy,
            DateTime createdAt,
            string modifiedBy,
            DateTime modifiedAt
          ) : base(id)
        {

            DateOfBirth = dateOfBirth;
            Gender = gender;
            ContactNumber = contactNumber;
            PermanentAddress = permanentAddress;
            EducationLevel = educationLevel;
            CompletionYear = completionYear;
            CurrentGpa = currentGpa;
            PreviousAcademicQualification = previousAcademicQualification;
            EnglishProficiency = englishProficiency;
            BandScore = bandScore;
            LanguageRemarks = languageRemarks;

            SkillOrTrainingName = SkillOrTrainingName;
            InstitutionName = InstitutionName;
            TrainingRemarks = trainingRemarks;
            TrainingStartDate = trainingStartDate;
            TrainingEndDate = trainingEndDate;



            Source = source;
            FeedBackOrSuggestion = feedBackOrSuggestion;
            IsActive = isActive;
            SchoolId = schoolId;
            CreatedBy = createdBy;
            CreatedAt = createdAt;
            ModifiedBy = modifiedBy;
            ModifiedAt = modifiedAt;

        }

        public string? SkillOrTrainingName { get; set; }
        public string? InstitutionName { get; set; }
        public string? TrainingRemarks { get; set; }
        public string? TrainingStartDate { get; set; }
        public string? TrainingEndDate { get; set; }

        public EnglishProficiency? EnglishProficiency { get; set; }
        public decimal? BandScore { get; set; }
        public string? LanguageRemarks { get; set; }
        public string SchoolId { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;

        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get;set; }
        public string ContactNumber { get;set; }
        public string PermanentAddress { get;set; }
        public EducationLevel EducationLevel { get; set; }
        public string CompletionYear { get; set; }
        public string CurrentGpa { get; set; }
        public string PreviousAcademicQualification { get; set; }

        public string Source { get; set; }
        public string FeedBackOrSuggestion { get; set; }
        public virtual UserProfile Profile { get; set; }
    }
}
