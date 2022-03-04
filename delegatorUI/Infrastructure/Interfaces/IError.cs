using System.Threading.Tasks;

namespace delegatorUI.Infrastructure.Interfaces
{
    public interface IError
    {
        public string ErrorText { get; set; }

        public int ErrorOpacity { get; set; }

        public async void Error(string errorText)
        {
            ErrorText = errorText;
            ErrorOpacity = 1;
            await Task.Delay(5000);
            ErrorOpacity = 0;
        }
    }
}
