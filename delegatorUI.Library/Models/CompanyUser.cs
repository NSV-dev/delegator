namespace delegatorUI.Library.Models
{
    public partial class CompanyUser
    {
        public string CompanyId { get; set; }
        public string AppUserId { get; set; }
        public string RoleId { get; set; }

        public virtual User User { get; set; }
        public virtual Company Company { get; set; }
        public virtual Role Role { get; set; }
    }
}
