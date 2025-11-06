using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Entities.AuditLogs;

namespace TN.Shared.Application.ServiceInterface.IBackGroundService
{
    public interface IAuditServices
    {
        Task RecordChangesAsync(IEnumerable<AuditLog> auditLogs);
        IEnumerable<AuditLog> DequeueAll();
    }
}
