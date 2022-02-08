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
    }
}
