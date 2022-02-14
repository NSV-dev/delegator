using delegatorApi.Library.Models;
using delegatorApi.Library.Models.Context;

namespace delegatorApi.Library.DataAccess
{
    public class TaskTaskData
    {
        public void Post(TasksTask tasksTask)
        {
            using (delegatorContext db = new())
            {
                db.TasksTasks.Add(tasksTask);
                db.SaveChanges();
            }
        }
    }
}
