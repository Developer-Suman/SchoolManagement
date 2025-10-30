﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Primitive;

namespace TN.Inventory.Domain.Entities
{
    public class ConversionFactor: Entity
    {
        public ConversionFactor(): base(null)
        {
            
        }

        public ConversionFactor(
            string id,
            string name,
            string fromUnit,
            string toUnit,
            decimal conversionFactor,
            DateTime createdAt,
            string userId,
            string updateBy,
            DateTime updatedAt,
            string schoolId
            ): base(id)
        {
            Name = name;
            FromUnit = fromUnit;
            ToUnit = toUnit;
            ConversionFactors = conversionFactor;
            CreatedAt = createdAt;
            UserId = userId;
            UpdateBy = updateBy;
            UpdatedAt = updatedAt;
            SchoolId = schoolId;
        
            Items = new List<Items>();


        }

        public string Name { get; set; }
        public string FromUnit { get; set; }
        public string ToUnit { get; set; }
        public decimal ConversionFactors {  get; set; }
        public DateTime CreatedAt { get; set;}
        public string UserId { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdatedAt { get; set;}

        //Navigation Property
        public Units FromUnits { get; set; } = null!;
        public Units ToUnits { get; set; } = null!;
        public string SchoolId { get; set; }
    
        public ICollection<Items> Items { get; set; }
    }
}
