namespace delegatorUI.Library.Models
{
    public class ComplitedFile
    {
        public string Id { get; set; }
        public string ComplitedId { get; set; }
        public string FileId { get; set; }

        public virtual Complited Complited { get; set; }
        public virtual AppFile File { get; set; }
    }
}
