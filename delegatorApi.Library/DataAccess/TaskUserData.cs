using delegatorApi.Library.Models;
using delegatorApi.Library.Models.Context;
using System.Linq;

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

        public void Update(TasksUser tasksUser)
        {
            using (delegatorContext db = new())
            {
                db.TasksUsers.Update(tasksUser);
                db.SaveChanges();
            }
        }

        public TasksUser Get(TasksUser tasksUser)
        {
            using (delegatorContext db = new())
            {
                var list = db.TasksUsers.Where(tu =>
                    tu.CompanyId == tasksUser.CompanyId &&
                    tu.TaskId == tasksUser.TaskId &&
                    tu.UserId == tasksUser.UserId);
                if (list.Count() == 0)
                    return null;
                else
                    return list.Single();
            }
        }
    }
}
