using System;

#nullable disable

namespace delegator.Library.DBModels
{
    public partial class Task
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? EndTime { get; set; }
    }
}
