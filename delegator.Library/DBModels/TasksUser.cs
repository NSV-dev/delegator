#nullable disable

namespace delegator.Library.DBModels
{
    public partial class TasksUser
    {
        public string TaskId { get; set; }
        public string UserId { get; set; }
        public string CompanyId { get; set; }

        public virtual Company Company { get; set; }
        public virtual Task Task { get; set; }
        public virtual AppUser User { get; set; }
    }
}
