﻿using delegatorUI.Library.Api.Helpers.Base;
using delegatorUI.Library.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace delegatorUI.Library.Api.Helpers
{
    public class RoleHelper : BaseHelper
    {
        public RoleHelper(HttpClient apiClient) 
            : base(apiClient)
        {
        }

        public async Task<Role> GetByTitle(string title)
        {
            using (HttpResponseMessage resp = await _apiClient.GetAsync($"Role/ByTitle?title={title}"))
            {
                if (resp.IsSuccessStatusCode)
                    return await resp.Content.ReadAsAsync<Role>();
                else
                    throw new Exception(resp.ReasonPhrase);
            }
        }
    }
}
