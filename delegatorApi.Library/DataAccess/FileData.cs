using delegatorApi.Library.Models;
using delegatorApi.Library.Models.Context;
using System.Linq;

namespace delegatorApi.Library.DataAccess
{
    public class FileData
    {
        public string Post(File file)
        {
            using (delegatorContext db = new())
            {
                db.File.Add(file);
                db.SaveChanges();
                return file.Id.Trim('"', '\\');
            }
        }

        public File GetById(string fileID)
        {
            using (delegatorContext db = new())
            {
                return db.File.Where(c => c.Id == fileID).Single();
            }
        }
    }
}
