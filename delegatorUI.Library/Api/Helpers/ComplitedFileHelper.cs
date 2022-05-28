using delegatorUI.Library.Api.Helpers.Base;
using delegatorUI.Library.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace delegatorUI.Library.Api.Helpers
{
    public class ComplitedFileHelper : BaseHelper
    {
        public ComplitedFileHelper(HttpClient apiClient) : base(apiClient)
        { }

        public async Task Post(ComplitedFile complitedFile)
        {
            using (HttpResponseMessage resp = await _apiClient.PostAsJsonAsync("ComplitedFile", complitedFile))
            {
                if (resp.IsSuccessStatusCode)
                    return;
                else
                    throw new Exception(resp.ReasonPhrase);
            }
        }

        public async Task<List<ComplitedFile>> GetByComplited(string complitedID)
        {
            using (HttpResponseMessage resp = await _apiClient.GetAsync($"ComplitedFile/ByComplited?complitedID={complitedID}"))
            {
                if (resp.IsSuccessStatusCode)
                    return await resp.Content.ReadAsAsync<List<ComplitedFile>>();
                else
                    throw new Exception(resp.ReasonPhrase);
            }
        }
    }
}
