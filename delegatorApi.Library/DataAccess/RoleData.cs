using delegatorApi.Library.Models;
using delegatorApi.Library.Models.Context;
using System.Collections.Generic;
using System.Linq;

namespace delegatorApi.Library.DataAccess
{
    public class RoleData
    {
        public List<Role> Get()
        {
            using (delegatorContext db = new())
            {
                return db.Roles.ToList();
            }
        }

        public Role GetByTitle(string title)
        {
            using (delegatorContext db = new())
            {
                return db.Roles.First(r => r.Title.ToLower() == title.ToLower());
            }
        }
    }
}
