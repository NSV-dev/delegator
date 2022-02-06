using delegatorUI.Library.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace delegatorUI.Library.Api
{
    public class APIHelper
    {
        private HttpClient _apiClient;

        public APIHelper()
        {
            InitializeClient();
        }

        private void InitializeClient()
        {
            _apiClient = new();
            _apiClient.BaseAddress = new Uri(ConfigurationManager.AppSettings["api"]);
            _apiClient.DefaultRequestHeaders.Accept.Clear();
            _apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<List<User>> GetAllUsers()
        {
            using (HttpResponseMessage resp = await _apiClient.GetAsync("/api/AppUser"))
            {
                if (resp.IsSuccessStatusCode)
                    return await resp.Content.ReadAsAsync<List<User>>();
                else
                    throw new Exception(resp.ReasonPhrase);
            }
        }
    }
}
