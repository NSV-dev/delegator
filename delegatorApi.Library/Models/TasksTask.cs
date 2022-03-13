#nullable disable

namespace delegatorApi.Library.Models
{
    public partial class TasksTask
    {
        public string Id { get; set; }
        public string MainTaskId { get; set; }
        public string TaskId { get; set; }

        public virtual Task MainTask { get; set; }
        public virtual Task Task { get; set; }
    }
}
