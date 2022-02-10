using delegatorApi.Library.Models;
using delegatorApi.Library.Models.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace delegatorApi.Library.DataAccess
{
    public class RoleData
    {
        public Role GetByTitle(string title)
        {
            using (delegatorContext db = new())
            {
                return db.Roles.First(r => r.Title.ToLower() == title.ToLower());
            }
        }
    }
}
