using delegatorUI.Library.Api.Helpers.Base;
using delegatorUI.Library.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace delegatorUI.Library.Api.Helpers
{
    public class AppUserHelper : BaseHelper
    {
        public AppUserHelper(HttpClient apiClient) 
            : base(apiClient) { }

        public async Task<List<User>> GetAll()
        {
            using (HttpResponseMessage resp = await _apiClient.GetAsync("AppUser"))
            {
                if (resp.IsSuccessStatusCode)
                    return await resp.Content.ReadAsAsync<List<User>>();
                else
                    throw new Exception(resp.ReasonPhrase);
            }
        }

        public async Task<User> GetByUsername(string name)
        {
            using (HttpResponseMessage resp = await _apiClient.GetAsync($"AppUser/ByUsername?name={name}"))
            {
                if (resp.IsSuccessStatusCode)
                    return await resp.Content.ReadAsAsync<User>();
                else
                    throw new Exception(resp.ReasonPhrase);
            }
        }
    }
}
