using delegatorApi.Library.Models;
using delegatorApi.Library.Models.Context;
using System.Collections.Generic;
using System.Linq;

namespace delegatorApi.Library.DataAccess
{
    public class CategoryData
    {
        public List<Category> GetAll()
        {
            using (delegatorContext db = new())
            {
                return db.Categories.ToList();
            }
        }

        public Category GetByTitle(string title)
        {
            using (delegatorContext db = new())
            {
                return db.Categories.Where(c => c.Title == title).Single();
            }
        }
    }
}
