using delegatorApi.Library.Models;
using delegatorApi.Library.Models.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace delegatorApi.Library.DataAccess
{
    public class ComplitedFileData
    {
        public void Post(ComplitedFile complitedFile)
        {
            using (delegatorContext db = new())
            {
                db.ComplitedFile.Add(complitedFile);
                db.SaveChanges();
            }
        }

        public List<ComplitedFile> GetByComplited(string complitedID)
        {
            using (delegatorContext db = new())
            {
                return db.ComplitedFile.Where(c => c.ComplitedId == complitedID)
                    .Include(c => c.File)
                    .ToList();
            }
        }
    }
}
