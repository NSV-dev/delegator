using System;
using System.Collections.Generic;

#nullable disable

namespace delegatorApi.Library.Models
{
    public partial class AppUser
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
    }
}
