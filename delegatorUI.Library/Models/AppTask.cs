using System;
using System.Collections.Generic;

namespace delegatorUI.Library.Models
{
    public class AppTask
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime EndTime { get; set; }
        public string SenderId { get; set; }
        public string CategoryId { get; set; }

        public Category Category { get; set; }
        public AppUser Sender { get; set; }
        public List<AppUser> Users { get; set; }
        public List<AppTask> Tasks { get; set; }
    }
}
