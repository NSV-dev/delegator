using System;

#nullable disable

namespace delegatorApi.Library.Models
{
    public partial class Task
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? EndTime { get; set; }
        public string SenderId { get; set; }
        public string CategoryId { get; set; }
        public string ResponsibleId { get; set; }

        public virtual Category Category { get; set; }
        public virtual AppUser Sender { get; set; }
        public virtual AppUser Responsible { get; set; }
    }
}
