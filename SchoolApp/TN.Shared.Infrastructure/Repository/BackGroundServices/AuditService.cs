using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Application.ServiceInterface.IBackGroundService;
using TN.Shared.Domain.Entities.AuditLogs;

namespace TN.Shared.Infrastructure.Repository.BackGroundServices
{
    public class AuditService : IAuditServices
    {
        private readonly ConcurrentQueue<AuditLog> _auditLogQueue = new();

        public IEnumerable<AuditLog> DequeueAll()
        {
            var logs = new List<AuditLog>();
            while (_auditLogQueue.TryDequeue(out var log))
            {
                logs.Add(log);
            }
            return logs;
        }

        public Task RecordChangesAsync(IEnumerable<AuditLog> auditLogs)
        {
            foreach (var log in auditLogs)
            {
                _auditLogQueue.Enqueue(log);
            }

            return Task.CompletedTask;
        }


    }
}
