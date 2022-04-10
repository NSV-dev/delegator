using delegatorApi.Library.Models;
using delegatorApi.Library.Models.Context;
using System.Collections.Generic;
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

        public void Update(TasksTask tasksTask)
        {
            using (delegatorContext db = new())
            {
                db.TasksTasks.Update(tasksTask);
                db.SaveChanges();
            }
        }

        public TasksTask GetByTaskID(string taskId)
        {
            using (delegatorContext db = new())
            {
                List<TasksTask> list = db.TasksTasks.Where(tt => tt.TaskId == taskId).ToList();
                if (list.Count() == 0)
                    return null;
                else
                    return list.Single();
            }
        }
    }
}
