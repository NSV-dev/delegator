using delegatorApi.Library.Models;
using delegatorApi.Library.Models.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace delegatorApi.Library.DataAccess
{
    public class CompanyData
    {
        public List<Company> GetByUserId(string id)
        {
            using (delegatorContext db = new())
            {
                return db.CompanyUsers.Where(cu => cu.AppUserId == id).Select(cu => cu.Company).ToList();
            }
        }
    }
}
