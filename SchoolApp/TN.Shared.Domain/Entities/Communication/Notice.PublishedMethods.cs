using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Shared.Domain.Entities.Communication
{
    public partial class Notice
    {
        public void Update(
            string title,
            string contentHtml,
            string? shortDescription,
            string? coverImgUrl,
            string modifiedBy
        )
        {
            if (IsPublished)
                throw new InvalidOperationException(
                    "Published notice cannot be modified."
                );

            Title = title;
            ContentHtml = contentHtml;
            ShortDescription = shortDescription;

            ModifiedBy = modifiedBy;
            ModifiedAt = DateTime.UtcNow;
        }

        public void Publish(string user)
        {
            if (IsPublished)
                return;

            IsPublished = true;
            PublishedAt = DateTime.UtcNow;

            ModifiedBy = user;
            ModifiedAt = DateTime.UtcNow;
        }

        public void Unpublish(string user)
        {
            if (!IsPublished)
                return;

            IsPublished = false;
            PublishedAt = null;

            ModifiedBy = user;
            ModifiedAt = DateTime.UtcNow;
        }




    }
}
