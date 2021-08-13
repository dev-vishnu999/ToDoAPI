using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IToDoItemService
    {
        Task<IEnumerable<ToDoItemModel>> GetItems(string userId);
        Task<dynamic> UpsertToDoItem(ToDoItemModel toDoItemModel, string userId);
        Task<dynamic> DeleteItem(int id);
    }
}
