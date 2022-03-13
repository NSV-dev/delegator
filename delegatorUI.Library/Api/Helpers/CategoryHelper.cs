using delegatorUI.Library.Api.Helpers.Base;
using delegatorUI.Library.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace delegatorUI.Library.Api.Helpers
{
    public class CategoryHelper : BaseHelper
    {
        public CategoryHelper(HttpClient apiClient)
            : base(apiClient)
        { }

        public async Task<List<Category>> GetAll()
        {
            using (HttpResponseMessage resp = await _apiClient.GetAsync("Category"))
            {
                if (resp.IsSuccessStatusCode)
                    return await resp.Content.ReadAsAsync<List<Category>>();
                else
                    throw new Exception(resp.ReasonPhrase);
            }
        }

        public async Task<Category> GetByTitle(string title)
        {
            using (HttpResponseMessage resp = await _apiClient.GetAsync($"Category/ByTitle?title={title}"))
            {
                if (resp.IsSuccessStatusCode)
                    return await resp.Content.ReadAsAsync<Category>();
                else
                    throw new Exception(resp.ReasonPhrase);
            }
        }
    }
}
