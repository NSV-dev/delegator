using System.Net.Http;

namespace delegatorUI.Library.Api.Helpers.Base
{
    public class BaseHelper
    {
        protected readonly HttpClient _apiClient;

        public BaseHelper(HttpClient apiClient)
        {
            _apiClient = apiClient;
        }
    }
}
