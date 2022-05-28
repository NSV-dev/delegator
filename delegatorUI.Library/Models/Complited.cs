#nullable disable

using System;

namespace delegatorUI.Library.Models
{
    public partial class Complited
    {
        public string Id { get; set; }
        public string CompanyUserId { get; set; }
        public int Duration { get; set; }
        public DateTime? EndTime { get; set; }
        public string Comment { get; set; }
        public string TaskCode { get; set; }
        public string TaskTitle { get; set; }
        public string TaskDescription { get; set; }

        public virtual CompanyUser CompanyUser { get; set; }
    }
}
