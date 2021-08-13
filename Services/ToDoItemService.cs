using AutoMapper;
using Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ToDoItemService : IToDoItemService
    {
        private readonly IToDoItemRepository _toDoitemRepository;
        private readonly IMapper _mapper;
        public ToDoItemService(IToDoItemRepository toDoitemRepository, IMapper mapper)
        {
            this._toDoitemRepository = toDoitemRepository;
            this._mapper = mapper;
        }
        public Task<IEnumerable<ToDoItemModel>> GetItems(string userId)
        {
            var items = _mapper.Map<IEnumerable<ToDoItem>, IEnumerable<ToDoItemModel>>(_toDoitemRepository.GetAllItems(userId));
            return Task.FromResult(items);
        }

        public Task<dynamic> UpsertToDoItem(ToDoItemModel toDoItemModel, string userId)
        {
            var item = _mapper.Map<ToDoItemModel, ToDoItem>(toDoItemModel);
            return _toDoitemRepository.UpsertItem(item, userId);
        }

        public Task<dynamic> DeleteItem(int id)
        {
            return _toDoitemRepository.DeleteItem(id);
        }
    }
}
