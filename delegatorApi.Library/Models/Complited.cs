#nullable disable

using System;

namespace delegatorApi.Library.Models
{
    public partial class Complited
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string TaskId { get; set; }
        public int Duration { get; set; }
        public DateTime? EndTime { get; set; }

        public virtual AppUser User { get; set; }
    }
}
