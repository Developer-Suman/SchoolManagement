using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Primitive;

namespace TN.Shared.Domain.Entities.Communication
{
    public partial class Notice : Entity
    {
        public Notice(): base(null)
        {
            
        }

        public Notice(
            string id,
            string title,
            string contentHtml,
            string? shortDescription,
            DateTime createdAt,
            string createdBy,
            DateTime modifiedAt,
            string modifiedBy,
            string schoolId,
            bool isActive
            ) : base(id)
        {
            Title = title;
            ContentHtml = contentHtml;
            ShortDescription = shortDescription;
            CreatedAt = createdAt;
            CreatedBy = createdBy;
            ModifiedAt = modifiedAt;
            ModifiedBy = modifiedBy;
            IsPublished = false;
            SchoolId = schoolId;
            IsActive = isActive;


        }

        public bool IsActive { get; set;  }
        public string SchoolId { get; set; }
        public string Title { get; set; }
        public string ContentHtml { get; set; }
        public string? ShortDescription { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedAt { get;set; }
        public string ModifiedBy { get; set; }
        public bool IsPublished { get; private set; }
        public DateTime? PublishedAt { get; private set; }

    }
}
