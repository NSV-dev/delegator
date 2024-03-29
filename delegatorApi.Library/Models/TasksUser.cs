﻿#nullable disable

namespace delegatorApi.Library.Models
{
    public partial class TasksUser
    {
        public string Id { get; set; }
        public string TaskId { get; set; }
        public string UserId { get; set; }
        public string CompanyId { get; set; }
        public string ToDo { get; set; }

        public virtual Company Company { get; set; }
        public virtual Task Task { get; set; }
        public virtual AppUser User { get; set; }
    }
}
