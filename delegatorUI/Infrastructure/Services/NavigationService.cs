using delegatorUI.Infrastructure.Stores;
using delegatorUI.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace delegatorUI.Infrastructure.Services
{
    public class NavigationService
    {
        private readonly NavigationStore _navigationStore;

        public NavigationService(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
        }

        public void Navigate()
        {

        }
    }
}
