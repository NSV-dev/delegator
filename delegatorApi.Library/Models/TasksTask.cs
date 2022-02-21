#nullable disable

using System.ComponentModel.DataAnnotations;

namespace delegatorApi.Library.Models
{
    public partial class TasksTask
    {
        public string MainTaskId { get; set; }
        [Key]
        public string TaskId { get; set; }

        public virtual Task MainTask { get; set; }
        public virtual Task Task { get; set; }
    }
}
