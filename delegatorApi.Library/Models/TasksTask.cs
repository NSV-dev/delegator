﻿#nullable disable

using System.ComponentModel.DataAnnotations;

namespace delegatorApi.Library.Models
{
    public partial class TasksTask
    {
        [Key]
        public string MainTaskId { get; set; }
        public string TaskId { get; set; }

        public virtual Task MainTask { get; set; }
        public virtual Task Task { get; set; }
    }
}
