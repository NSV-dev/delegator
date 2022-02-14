using System;
using System.Collections.Generic;

namespace delegatorUI.Library.Models
{
    public class AppTask
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? EndTime { get; set; }
        
        public Company Company { get; set; }
        public User User { get; set; }
        public List<AppTask> Tasks { get; set; }
    }
}
