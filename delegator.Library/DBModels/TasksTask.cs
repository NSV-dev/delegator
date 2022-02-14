﻿#nullable disable

namespace delegator.Library.DBModels
{
    public partial class TasksTask
    {
        public string MainTaskId { get; set; }
        public string TaskId { get; set; }

        public virtual Task MainTask { get; set; }
        public virtual Task Task { get; set; }
    }
}
