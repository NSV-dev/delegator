#nullable disable

namespace delegatorApi.Library.Models
{
    public partial class TasksUser
    {
        public string TaskId { get; set; }
        public string UserId { get; set; }

        public virtual Task Task { get; set; }
        public virtual AppUser User { get; set; }
    }
}
