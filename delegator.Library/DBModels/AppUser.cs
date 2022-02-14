#nullable disable

namespace delegator.Library.DBModels
{
    public partial class AppUser
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}
