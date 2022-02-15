﻿#nullable disable

using System.ComponentModel.DataAnnotations;

namespace delegatorApi.Library.Models
{
    public partial class TasksUser
    {
        [Key]
        public string TaskId { get; set; }
        public string UserId { get; set; }
        public string CompanyId { get; set; }

        public virtual Company Company { get; set; }
        public virtual Task Task { get; set; }
        public virtual AppUser User { get; set; }
    }
}