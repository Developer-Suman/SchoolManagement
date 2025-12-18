using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Primitive;
using static TN.Shared.Domain.Enum.SchoolItemEnum;

namespace TN.Shared.Domain.Entities.SchoolItems
{
    public class SchoolItemsHistory : Entity
    {
        public SchoolItemsHistory(
            ): base(null)
        {
            
        }

        public SchoolItemsHistory(
            string id, 
            string schoolItemId,
            ItemStatus previousStatus,
            ItemStatus currentStatus,
            string? remarks,
            DateTime actionDate,
            string actionBy,
            string schoolId,
            bool isActive,
            string createdBy,
            DateTime createdAt,
            string modifiedBy,

            DateTime modifiedAt

            ) : base(id)
        {
            Id = id;
            SchoolItemId = schoolItemId;
            PreviousStatus = previousStatus;
            CurrentStatus = currentStatus;
            Remarks = remarks;
            ActionDate = actionDate;
            ActionBy = actionBy;
            IsActive = isActive;
            CreatedBy = createdBy;
            CreatedAt = createdAt;
            ModifiedBy = modifiedBy;
            ModifiedAt = modifiedAt;


        }

        public string SchoolItemId { get; set; }
        public SchoolItem SchoolItem { get; set; }
        public ItemStatus PreviousStatus { get; set; }
        public ItemStatus CurrentStatus { get; set; }
        public string? Remarks { get; set; }
        public DateTime ActionDate { get; set; }
        public string ActionBy { get; set; }
        public string SchoolId { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; }
    }
}
