using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Authentication.Domain.Entities;
using TN.Shared.Domain.Primitive;

namespace TN.Shared.Domain.Entities.OrganizationSetUp
{
    public class SchoolSettingsFiscalYear : Entity
    {
        public SchoolSettingsFiscalYear(

            ) : base(null)
        {

        }

        public SchoolSettingsFiscalYear(
            string id,
            string schoolSettingsId,
            string fiscalYearId,
            bool isClosed,
            string userId,
            DateTime closedAt,
            bool isUpToCurrentFiscalYear,
            string fyName,
            string schoolId,
            bool isFiscalYearStarted
            ) : base(id)
        {
            SchoolSettingsId = schoolSettingsId;
            FiscalYearId = fiscalYearId;
            IsClosed = isClosed;
            UserId = userId;
            ClosedAt = closedAt;
            IsUpToCurrentFiscalYear = isUpToCurrentFiscalYear;
            FyName = fyName;
            SchoolId = schoolId;
            IsFiscalYearStarted = isFiscalYearStarted;

        }

        public string SchoolSettingsId { get; set; }
        public SchoolSettings SchoolSettings { get; set; }

        public string FiscalYearId { get; set; }
        public FiscalYears FiscalYear { get; set; }

        public bool IsClosed { get; set; }
        public string UserId { get; set; }
        public DateTime ClosedAt { get; set; }
        public bool IsUpToCurrentFiscalYear { get; set; }
        public bool IsFiscalYearStarted { get; set; }

        public string FyName { get; set; }
        public string SchoolId { get; set; }
    }
}
        
