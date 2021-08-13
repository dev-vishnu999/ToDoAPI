using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class AuditLogRepository : BaseRepository<AuditLog>, IAuditLogRepository
    {
        AppDbContext _context;
        public AuditLogRepository(AppDbContext context)
            : base(context)
        {
            _context = context;
        }
        public Task<int> UpdateLog(AuditLog log)
        {
            _context.AuditLog.AddAsync(log);
            _context.SaveChangesAsync();
            return Task.FromResult(log.Id);
        }
    }
}
