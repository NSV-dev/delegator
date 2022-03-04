using delegatorUI.Library.Api;
using delegatorUI.Library.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace delegatorUI.Infrastructure.Stores
{
    public class CompanyUserStore
    {
        private readonly APIHelper _apiHelper;

        public CompanyUser CompanyUser { get; set; }

        public CompanyUserStore(APIHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task LoadCompanyUser(string companyID, string userID)
        {
            CompanyUser = (await _apiHelper.CompaniesUsers.GetByCompanyId(companyID, userID)).Single();
            CompanyUserChanged?.Invoke(this, new EventArgs());
        }

        public event EventHandler CompanyUserChanged;
    }
}
