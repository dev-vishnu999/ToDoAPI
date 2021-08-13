using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TODOApi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ToDoController : ControllerBase
    {
        private readonly IToDoItemService _toDoitemService;
        //private readonly IAuditLogRepository _auditLogRepository;
        public ToDoController(IToDoItemService toDoitemService)//, IAuditLogRepository auditLogRepository)
        {
            this._toDoitemService = toDoitemService;
            //_auditLogRepository = auditLogRepository;
        }
        [HttpGet("getItems")]
        public async Task<dynamic> GetItems()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return new { success = true, data = await _toDoitemService.GetItems(userId), message ="" };
        }

        [HttpPost("upsertToDoItem")]
        public async Task<dynamic> UpsertToDoItem([FromBody] ToDoItemModel model)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var res = await _toDoitemService.UpsertToDoItem(model, userId);
            return new { success = true, data = "", message = res };
            //var jobId = BackgroundJob.Enqueue(() => _auditLogRepository.UpdateLog(res.AuditLog));
            //return Ok(new Response(true, "", res.Id));
        }

        [HttpPost("deleteItem")]
        public async Task<dynamic> DeleteItem([FromBody] ToDoItemModel model) 
        {
            var res = await _toDoitemService.DeleteItem(model.Id);
            return new { success = true, data = "", message = res };
        }
    }
}
