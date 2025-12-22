using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Entities.OrganizationSetUp;
using TN.Shared.Domain.Primitive;
using static TN.Shared.Domain.Enum.SchoolItemEnum;

namespace TN.Shared.Domain.Entities.SchoolItems
{
    public class SchoolItem: Entity
    {
        public SchoolItem(
            ): base(null)
        {
            
        }
            
        public SchoolItem(
            string id,
            string name,
            string contributorId,
            ItemStatus itemStatus,
            ItemCondition itemCondition,
            DateTime receivedDate,
            decimal? estimatedValue,
            decimal? quantity,
            UnitType? unitType,
            string schoolId,
            string fiscalYearId,
            bool isActive,
            string createdBy,
            DateTime createdAt,
            string modifiedBy,

            DateTime modifiedAt

            ) : base(id)
        {
            Id = id;
            Name = name;
            ContributorId = contributorId;
            ItemStatus = itemStatus;
            ItemCondition = itemCondition;
            ReceivedDate = receivedDate;
            EstimatedValue = estimatedValue;
            Quantity = quantity;
            UnitType = unitType;
            IsActive = isActive;
            SchoolId = schoolId;
            FiscalYearId = fiscalYearId;
            CreatedBy = createdBy;
            CreatedAt = createdAt;
            ModifiedBy = modifiedBy;
            ModifiedAt = modifiedAt;
            SchoolItemsHistories = new List<SchoolItemsHistory>();



        }


        public UnitType? UnitType { get; set; }
        public decimal? Quantity { get; set; }
        public string Name { get; set; }
        //public string CategoryId { get; set; }

        public string ContributorId { get;set; }
        public Contributor Contributor { get; set; }
        public ItemStatus ItemStatus { get; set; }
        public ItemCondition ItemCondition { get; set; }
        public DateTime ReceivedDate { get; set; }
        public decimal? EstimatedValue { get; set; }
        public string SchoolId { get; set; }
        public string? FiscalYearId { get; set; }
        public FiscalYears FiscalYear { get; set; } = default!;
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; }

        public ICollection<SchoolItemsHistory>  SchoolItemsHistories { get; set; }
    }
}
