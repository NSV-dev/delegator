namespace delegatorUI.Library.Models
{
    public class User
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

        public User()
        {

        }

        public User(AppUser user)
        {
            Id = user.Id;
            UserName = user.UserName;
            Password = user.Password;
            Email = user.Email;
        }
    }
}
