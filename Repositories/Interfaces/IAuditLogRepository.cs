using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IAuditLogRepository : IBaseRepository<AuditLog>
    {
        Task<int> UpdateLog(AuditLog log);
    }
}
