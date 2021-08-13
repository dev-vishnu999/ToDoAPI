using System;
using System.Collections.Generic;
using System.Text;

namespace Repositories
{
    public class AuditLog
    {
        public AuditLog()
        {

        }
        public AuditLog(string accountId, string action, int itemid, string oldvalue, string newValue)
        {
            this.AccountId = accountId;
            this.Action = action;
            this.TodoItemId = itemid;
            this.OldValues = oldvalue;
            this.NewValues = newValue;
        }
        public int Id { get; set; }
        public string AccountId { get; set; }
        public string Action { get; set; }
        public int TodoItemId { get; set; }
        public string OldValues { get; set; }
        public string NewValues { get; set; }
    }
}
