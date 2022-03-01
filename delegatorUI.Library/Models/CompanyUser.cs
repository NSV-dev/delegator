using Newtonsoft.Json;

namespace delegatorUI.Library.Models
{
    public partial class CompanyUser
    {
        public string Id { get; set; }
        public string CompanyId { get; set; }
        public string AppUserId { get; set; }
        public string RoleId { get; set; }

        [JsonProperty("AppUser")]
        public virtual AppUser User { get; set; }
        public virtual Company Company { get; set; }
        public virtual Role Role { get; set; }
    }
}
