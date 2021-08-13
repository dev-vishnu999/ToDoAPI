using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IToDoItemRepository : IBaseRepository<ToDoItem>
    {
        IEnumerable<ToDoItem> GetAllItems(string userId);
        Task<dynamic> UpsertItem(ToDoItem item, string userId);
        Task<dynamic> DeleteItem(int Id);
    }
}
