using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace delegatorUI.ViewResources
{
    /// <summary>
    /// Логика взаимодействия для BindablePasswordBox.xaml
    /// </summary>
    public partial class BindablePasswordBox : UserControl
    {
        private bool _isPasswordChanging;

        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register("Password", typeof(string), typeof(BindablePasswordBox),
                new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    PasswordPropertyChanged, null, false, UpdateSourceTrigger.PropertyChanged));

        private static void PasswordPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is BindablePasswordBox passwordBox)
            {
                passwordBox.UpdatePassword();
            }
        }

        public string Password
        {
            get { return (string)GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }

        public static new readonly DependencyProperty ForegroundProperty =
            DependencyProperty.Register("Foreground", typeof(SolidColorBrush), typeof(BindablePasswordBox),
                new FrameworkPropertyMetadata(new SolidColorBrush(), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    ForegroundPropertyChanged, null, false, UpdateSourceTrigger.PropertyChanged));

        private static void ForegroundPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is BindablePasswordBox passwordBox)
            {
                passwordBox.passwordBox.Foreground = e.NewValue as SolidColorBrush;
            }
        }

        public new SolidColorBrush Foreground
        {
            get => GetValue(ForegroundProperty) as SolidColorBrush;
            set => SetValue(ForegroundProperty, value);
        }

        public static readonly DependencyProperty CaretBrushProperty =
            DependencyProperty.Register("CaretBrush", typeof(SolidColorBrush), typeof(BindablePasswordBox),
                new FrameworkPropertyMetadata(new SolidColorBrush(), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    CaretBrushPropertyChanged, null, false, UpdateSourceTrigger.PropertyChanged));

        private static void CaretBrushPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is BindablePasswordBox passwordBox)
            {
                passwordBox.passwordBox.CaretBrush = e.NewValue as SolidColorBrush;
            }
        }

        public SolidColorBrush CaretBrush
        {
            get => GetValue(CaretBrushProperty) as SolidColorBrush;
            set => SetValue(CaretBrushProperty, value);
        }

        public BindablePasswordBox()
        {
            InitializeComponent();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            _isPasswordChanging = true;
            Password = passwordBox.Password;
            _isPasswordChanging = false;
        }

        private void UpdatePassword()
        {
            if (!_isPasswordChanging)
            {
                passwordBox.Password = Password;
            }
        }
    }
}
