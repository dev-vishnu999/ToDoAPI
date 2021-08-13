using System;
using System.Collections.Generic;
using System.Text;

namespace Services
{
    public class ToDoItemModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string AccountId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifedOn { get; set; }
        public string ModifedBy { get; set; }
        public string CreatedBy { get; set; }
        public bool Status { get; set; }
    }
}
