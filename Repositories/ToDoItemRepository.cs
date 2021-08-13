using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class ToDoItemRepository : BaseRepository<ToDoItem>, IToDoItemRepository
    {
        AppDbContext _context;
        private readonly IAuditLogRepository _auditLogRepository;
        public ToDoItemRepository(AppDbContext context, IAuditLogRepository auditLogRepository)
            : base(context)
        {
            _context = context;
            _auditLogRepository = auditLogRepository;
        }

        public IEnumerable<ToDoItem> GetAllItems(string userId)
        {
            return _context.ToDoItem.Where(x => x.AccountId == userId).OrderByDescending(x => x.CreatedAt);
        }

        public async Task<dynamic> UpsertItem(ToDoItem model, string userId)
        {
            AuditLog newLog = new AuditLog();
            string message = string.Empty;
            string newValues = "NewTitle = " + model.Title + ", NewDescription = " + model.Description;
            if (model.Id != 0)
            {
                var entity = _context.ToDoItem.FirstOrDefault(item => item.Id == model.Id);
                if (entity != null)
                {
                    entity.Title = model.Title;
                    entity.Description = model.Description;
                    entity.CreatedAt = entity.CreatedAt;
                    entity.ModifedOn = DateTime.UtcNow;
                    entity.ModifedBy = userId;
                    entity.Status = model.Status;
                    await _context.SaveChangesAsync();
                    string oldValues = "OldTitle = " + entity.Title + ", OldDescription = " + entity.Description;
                    message = "Item updated successfully";
                    newLog = new AuditLog(userId, "Edit Item", entity.Id, oldValues, newValues);
                }
            }
            else
            {
                model.AccountId = userId;
                model.CreatedBy = userId;
                model.CreatedAt = DateTime.UtcNow;
                model.Status = false;
                await _context.ToDoItem.AddAsync(model);
                await _context.SaveChangesAsync();
                message = "Item added successfully";
                newLog = new AuditLog(userId, "Add New Item", model.Id, "", newValues);
            }
            var jobId = BackgroundJob.Enqueue(() => _auditLogRepository.UpdateLog(newLog));
            return message;
        }

        public async Task<dynamic> DeleteItem(int Id)
        {
            string message = string.Empty;
            var entity = _context.ToDoItem.FirstOrDefault(item => item.Id == Id);
            if (entity != null)
            {
                _context.ToDoItem.Remove(entity);
                _context.SaveChanges();
                message = "Item deleted successfully";
            }
            return message;
        }
    }
}
