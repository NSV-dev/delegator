using System.ComponentModel;

namespace delegatorUI.Library.Models
{
    public class AppUserWithStats : INotifyPropertyChanged
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

        private int _complitedCount;
        public int ComplitedCount
        {
            get => _complitedCount;
            set
            {
                _complitedCount = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(ComplitedCount)));
            }
        }

        private int _complitedDuration;
        public int ComplitedDuration
        {
            get => _complitedDuration;
            set
            {
                _complitedDuration = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(ComplitedDuration)));
            }
        }

        public Role Role { get; set; }

        public AppUserWithStats(AppUser appUser)
        {
            Id = appUser.Id;
            UserName = appUser.UserName;
            Password = appUser.Password;
            Email = appUser.Email;
            Role = appUser.Role;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}

