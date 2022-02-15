using delegatorApi.Library.Models;
using delegatorApi.Library.Models.Context;
using System.Collections.Generic;
using System.Linq;

namespace delegatorApi.Library.DataAccess
{
    public class AppUserData
    {
        public void Post(AppUser appUser)
        {
            using (delegatorContext db = new())
            {
                db.AppUsers.Add(appUser);
                db.SaveChanges();
            }
        }

        public AppUser GetByUsername(string name)
        {
            using (delegatorContext db = new())
            {
                return db.AppUsers.FirstOrDefault(u => u.UserName == name);
            }
        }

        public List<AppUser> GetByCompanyId(string compId)
        {
            using (delegatorContext db = new())
            {
                return db.CompanyUsers.Where(cu => cu.CompanyId == compId).Select(x => x.AppUser).ToList();
            }
        }

        public List<AppUser> GetByTask(string taskID)
        {
            using (delegatorContext db = new())
            {
                return db.TasksUsers.Where(tu => tu.TaskId == taskID).Select(tu => tu.User).ToList();
            }
        }
    }
}
