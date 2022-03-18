using System;
using System.Windows;

namespace delegatorUI.View.Window
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ThemeBtn_Checked(object sender, RoutedEventArgs e)
        {
            ThemeBtn.Content = "☾";
            Application.Current.Resources.MergedDictionaries[0].Source = new Uri("\\viewresources\\themes\\dark.xaml", UriKind.RelativeOrAbsolute);
        }

        private void ThemeBtn_Unchecked(object sender, RoutedEventArgs e)
        {
            ThemeBtn.Content = "☀";
            Application.Current.Resources.MergedDictionaries[0].Source = new Uri("\\viewresources\\themes\\light.xaml", UriKind.RelativeOrAbsolute);
        }
    }
}
