using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Setup.Domain.Entities;
using TN.Shared.Domain.Primitive;

namespace TN.Shared.Domain.Entities.Setup
{
    public class AppName : Entity
    {
        public AppName(
            ): base(null)
        {
            
        }

        public AppName(
            string id,
            string name,
            string? description,
            string? logoUrl
            ) : base(id)
        {
            Name = name;
            Description = description;
            LogoUrl = logoUrl;
            Modules = new List<Modules>();
        }
        public string Name { get;set; }
        public string? Description { get;set; }
        public string? LogoUrl { get; set; }
        public ICollection<Modules> Modules { get; set; }
    }
}
