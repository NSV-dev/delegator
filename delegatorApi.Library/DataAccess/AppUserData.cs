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
        public AppUser GetUserById(string Id)
        {
            using (delegatorContext db = new())
            {
                return db.AppUsers.First(u => u.Id == Id);
            }
        }
    }
}
