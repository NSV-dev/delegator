using delegatorApi.Library.Models;
using delegatorApi.Library.Models.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace delegatorApi.Library.DataAccess
{
    public class AppUserData
    {
        public List<AppUser> GetAll()
        {
            using (delegatorContext db = new())
            {
                return db.AppUsers.ToList();
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
    }
}
