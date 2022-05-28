namespace delegatorApi.Library.Models
{
    public partial class ComplitedFile
    {
        public string Id { get; set; }
        public string ComplitedId { get; set; }
        public string FileId { get; set; }

        public virtual Complited Complited { get; set; }
        public virtual File File { get; set; }
    }
}
