using delegatorUI.Library.Api.Helpers.Base;
using delegatorUI.Library.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace delegatorUI.Library.Api.Helpers
{
    public class CompaniesHelper : BaseHelper
    {
        public CompaniesHelper(HttpClient apiClient)
            : base(apiClient) { }

        public async Task Post(Company company)
        {
            using (HttpResponseMessage resp = await _apiClient.PostAsJsonAsync("Company", company))
            {
                if (resp.IsSuccessStatusCode)
                    return;
                else
                    throw new Exception(resp.ReasonPhrase);
            }
        }

        public async Task<List<Company>> GetByUserId(string id)
        {
            using (HttpResponseMessage resp = await _apiClient.GetAsync($"Company/ByUserId?id={id}"))
            {
                if (resp.IsSuccessStatusCode)
                    return await resp.Content.ReadAsAsync<List<Company>>();
                else
                    throw new Exception(resp.ReasonPhrase);
            }
        }

        public async Task<Company> GetByTitle(string title)
        {
            using (HttpResponseMessage resp = await _apiClient.GetAsync($"Company/ByTitle?title={title}"))
            {
                if (resp.IsSuccessStatusCode)
                    return await resp.Content.ReadAsAsync<Company>();
                else
                    throw new Exception(resp.ReasonPhrase);
            }
        }

        public async Task<List<Company>> GetWhereTitleContains(string s)
        {
            if (string.IsNullOrWhiteSpace(s))
                return null;
            using (HttpResponseMessage resp = await _apiClient.GetAsync($"Company/WhereTitleContains?s={s}"))
            {
                if (resp.IsSuccessStatusCode)
                    return await resp.Content.ReadAsAsync<List<Company>>();
                else
                    throw new Exception(resp.ReasonPhrase);
            }
        }
    }
}
