﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Primitive;

namespace TN.Shared.Domain.Entities.Academics
{
    public class Subject : Entity
    {
        public Subject(
            ) : base(null)
        {

        }
        public Subject(
            string id,
            string name,
            string code,
            int? creditHours,
            string? description,
            string classId,
            string createdBy,
            DateTime createdAt,
            string modifiedBy,
            DateTime modifiedAt
            ) : base(id)
        {
            Name = name;
            Code = code;
            CreditHours = creditHours;
            Description = description;
            ClassId = classId;
            CreatedBy = createdBy;
            CreatedAt = createdAt;
            ModifiedBy = modifiedBy;
            ModifiedAt = modifiedAt;
            ExamResults = new List<ExamResult>();
        }
        public string Name { get; set; }
        public string Code { get; set; }
        public string ClassId { get; set; }
        public Class Class { get; set; }
        public int? CreditHours { get; set; }
        public string? Description { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; } = DateTime.Now;
        public ICollection<ExamResult> ExamResults
        {
            get; set;
        }
    }
}
