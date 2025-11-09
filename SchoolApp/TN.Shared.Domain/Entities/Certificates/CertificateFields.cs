//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using TN.Shared.Domain.Primitive;

//namespace TN.Shared.Domain.Entities.Certificates
//{
//    public class CertificateFields : Entity
//    {
//        public CertificateFields(
//            ): base(null)
//        {
            
//        }
//        public CertificateFields(
//            string id,
//            string templateId,
//            string fieldName,
//            string? displayLabel,
//            string fieldType,
//            bool isRequired

//            ) :  base(id)
//        {
//            FieldName = fieldName;
//            DisplayLabel = displayLabel;
//            FieldType = fieldType;
//            IsRequired = isRequired;

//        }

//        public string TemplateId { get; set; }
//        public string FieldName { get; set; } = default!;
//        public string? DisplayLabel { get; set; }
//        public string FieldType { get; set; } = "text"; // text, date, image, etc.
//        public bool IsRequired { get; set; } = true;
//    }
//}
