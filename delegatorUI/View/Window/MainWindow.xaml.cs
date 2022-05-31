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
            clr = 1;
        }

        private int clr;

        private void ThemeBtn_Click(object sender, RoutedEventArgs e)
        {
            clr++;
            if (clr == 4)
                clr = 1;
            switch (clr)
            {
                case 1:
                    ThemeBtn.Content = "    ☾";
                    Application.Current.Resources.MergedDictionaries[0].Source = new Uri("\\viewresources\\themes\\dark.xaml", UriKind.RelativeOrAbsolute);
                    break;
                case 2:
                    ThemeBtn.Content = "    🌣";
                    Application.Current.Resources.MergedDictionaries[0].Source = new Uri("\\viewresources\\themes\\light.xaml", UriKind.RelativeOrAbsolute);
                    break;
                case 3:
                    ThemeBtn.Content = "   🌈";
                    Application.Current.Resources.MergedDictionaries[0].Source = new Uri("\\viewresources\\themes\\color.xaml", UriKind.RelativeOrAbsolute);
                    break;
            }
        }
    }
}
