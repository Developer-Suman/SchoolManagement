﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Entities.OrganizationSetUp;
using TN.Shared.Domain.Entities.Students;
using TN.Shared.Domain.Primitive;

namespace TN.Shared.Domain.Entities.Academics
{
    public class ClassSection : Entity
    {
        public ClassSection(): base(null)
        {

        }
        
        public ClassSection(
            string id,
            string name,
            string code,
            string classId,
            string sectionId,
            string teacherId,
            string fyId,
            string createdBy,
            DateTime createdAt,
            string modifiedBy,
            DateTime modifiedAt
            ) : base(id)
        {
            Name = name;
            Code = code;
            ClassId = classId;
            SectionId = sectionId;
            TeacherId = teacherId;
            FyId = fyId;
            CreatedBy = createdBy;
            CreatedAt = createdAt;
            ModifiedBy = modifiedBy;
            ModifiedAt = modifiedAt;
            Students = new List<StudentData>();
        }
        public string Name { get; set; }
        public string Code { get; set; }
        public string ClassId { get; set; }
        public Class Class { get; set; }

        public string SectionId { get; set; }
        public Section Section { get; set; }
        public string TeacherId { get; set; }
        public Teacher Teacher { get; set; }
        public string FyId { get; set; }
        public FiscalYears FiscalYears { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; } = DateTime.Now;

        public ICollection<StudentData> Students { get; set; }

    }
}
