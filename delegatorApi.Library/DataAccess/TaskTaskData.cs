using delegatorApi.Library.Models;
using delegatorApi.Library.Models.Context;
using System.Linq;

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

        public void Delete(TasksTask tasksTask)
        {
            using (delegatorContext db = new())
            {
                db.TasksTasks.Remove(tasksTask);
                db.SaveChanges();
            }
        }

        public TasksTask GetByTaskID(string taskId)
        {
            using (delegatorContext db = new())
            {
                var list = db.TasksTasks.Where(tt => tt.TaskId == taskId);
                if (list.Count() == 0)
                    return null;
                else
                    return list.Single();
            }
        }
    }
}
