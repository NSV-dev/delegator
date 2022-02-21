#nullable disable

using System.ComponentModel.DataAnnotations;

namespace delegatorApi.Library.Models
{
    public partial class CompanyUser
    {
        [Key]
        public string CompanyId { get; set; }
        [Key]
        public string AppUserId { get; set; }
        public string RoleId { get; set; }

        public virtual AppUser AppUser { get; set; }
        public virtual Company Company { get; set; }
        public virtual Role Role { get; set; }
    }
}
