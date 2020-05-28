using System.Windows;

namespace Course_Lena
{
    /// <summary>
    /// Interaction logic for WindowRegestration.xaml
    /// </summary>
    public partial class WindowRegestration : Window
    {
        public WindowRegestration()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
        public string FirstName => textBoxFirstName.Text;
        public string SecondName => textBoxSecondName.Text;
        public string PassWord => textBoxPassWord.Password;
        public string Login => textBoxLogin.Text;
    }

}
