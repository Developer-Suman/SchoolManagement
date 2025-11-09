using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Authentication.Domain.Entities;
using TN.Shared.Domain.Entities.Academics;
using TN.Shared.Domain.Entities.FeeAndAccounting;
using TN.Shared.Domain.Primitive;

namespace TN.Shared.Domain.Entities.OrganizationSetUp
{
    public class FiscalYears : Entity
    {
        public FiscalYears(
            ): base(null)
        {
            
        }

        public FiscalYears(
            string id,
            string fyName,
            DateTime startDate,
            DateTime endDate
            
            ) : base(id)
        {
            FyName = fyName;
            StartDate = startDate;
            EndDate = endDate;
            SchoolSettingsFiscalYears = new List<SchoolSettingsFiscalYear>();
            FeeStructures = new List<FeeStructure>();
            ClassSections = new List<ClassSection>();

        }

        public string FyName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ICollection<SchoolSettingsFiscalYear> SchoolSettingsFiscalYears { get; set; }
        public ICollection<FeeStructure> FeeStructures { get; set; }
        public ICollection<Exam> Exams { get; set; }
        public ICollection<ClassSection> ClassSections { get; set; }
    }
}
