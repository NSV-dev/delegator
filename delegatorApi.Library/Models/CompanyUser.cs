using System;
using System.Collections.Generic;

#nullable disable

namespace delegatorApi.Library.Models
{
    public partial class CompanyUser
    {
        public string CompanyId { get; set; }
        public string AppUserId { get; set; }
        public string RoleId { get; set; }

        public virtual AppUser AppUser { get; set; }
        public virtual Company Company { get; set; }
        public virtual Role Role { get; set; }
    }
}
