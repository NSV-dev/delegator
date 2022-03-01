namespace delegatorUI.Library.Models
{
    public class AppUser
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

        public Role Role { get; set; }

        public AppUser(User user)
        {
            Id = user.Id;
            UserName = user.UserName;
            Password = user.Password;
            Email = user.Email;
        }
    }
}
