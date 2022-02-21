using delegatorApi.Library.Models;
using delegatorApi.Library.Models.Context;

namespace delegatorApi.Library.DataAccess
{
    public class TaskUserData
    {
        public void Post(TasksUser tasksUser)
        {
            using (delegatorContext db = new())
            {
                db.TasksUsers.Add(tasksUser);
                db.SaveChanges();
            }
        }

        public void Delete(TasksUser tasksUser)
        {
            using (delegatorContext db = new())
            {
                db.TasksUsers.Remove(tasksUser);
                db.SaveChanges();
            }
        }
    }
}
