using delegatorUI.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace delegatorUI.ViewModel.WindowViewModel
{
    public class WindowViewModel : BaseViewModel
    {
        private string _title;
        public string Title 
        {
            get => _title;
            set => OnPropertyChanged(ref _title, value);
        }

        public WindowViewModel()
        {
            Title = "123";
        }
    }
}
