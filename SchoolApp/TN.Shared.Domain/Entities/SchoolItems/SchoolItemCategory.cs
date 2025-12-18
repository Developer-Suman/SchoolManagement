//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using TN.Shared.Domain.Primitive;

//namespace TN.Shared.Domain.Entities.SchoolItems
//{
//    public class SchoolItemCategory : Entity
//    {
//        public SchoolItemCategory(
//            ): base(null)
//        {
            
//        }
//        public SchoolItemCategory(
//            string id,
//            string name,
//            string? description
//            ): base(id)
//        {
//            Name = name;
//            Description = description;
//            SchoolItem = new List<SchoolItem>();
            
//        }
//        public string Name { get; set; }
//        public string? Description { get; set; }
//        public ICollection<SchoolItem> SchoolItem { get; set; }
//    }
//}
