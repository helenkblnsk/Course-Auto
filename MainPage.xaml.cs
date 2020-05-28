using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace Course_Lena
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class Page1 : Page
    {
        public static bool Accept;
        public static XDocument xDocumentPassword;

        public Page1()
        {
            InitializeComponent();
            xDocumentPassword = PasswordXML.GetXDocument();
            Accept = bool.Parse(xDocumentPassword.Element("PassWords").Attribute("Accept").Value);
        }
        //Переход на страницу фильтра данных
        private void ButtonFilter_Click_1(object sender, RoutedEventArgs e)
        {
            if (!Accept)
            {
                MessageBox.Show("Авторизуйтесь !");
                return;
            }
            NavigationService nav = NavigationService.GetNavigationService(this);
            nav.Navigate(new Uri("FilterData.xaml", UriKind.Relative));
        }

        //Переход на страницу добавления инфрмации
        private void ButtonAddData_Click_2(object sender, RoutedEventArgs e)
        {
            if (!Accept)
            {
                MessageBox.Show("Авторизуйтесь !");
                return;
            }
            NavigationService nav = NavigationService.GetNavigationService(this);
            nav.Navigate(new Uri("AddData.xaml", UriKind.Relative));
        }

        //Диалоговое окно авторизации
        private void buttonLog_In_Click(object sender, RoutedEventArgs e)
        {
            PassWordWindow passWordWindow = new PassWordWindow();
            if (passWordWindow.ShowDialog() == true)
            {
                if (MethodLog_In(passWordWindow.Login, passWordWindow.PassWord))
                {
                    MessageBox.Show("Авторизация успшешна");
                    Accept = true;
                    xDocumentPassword.Element("PassWords").Attribute("Accept").Value = true.ToString();
                    PasswordXML.SaveXMLPassword(xDocumentPassword);
                }
                else
                    MessageBox.Show("Неверный логин и/или пароль");
            }
            else
                MessageBox.Show("Авторизация не пройдена");
        }

        //Метод авторизации
        private static bool MethodLog_In(string login, string password)
        {
            var result = from data in xDocumentPassword.Element("PassWords").Elements("Account")
                         select new
                         {
                             Login = data.Element("Login").Value,
                             Password = data.Element("PassWord").Value
                         };
            foreach (var item in result)
            {
                if (item.Login==login && item.Password==password)
                {
                    return true;
                }
            }
            return false;
        }

        //Метод регестрации
        private static bool Regestration(string first_name,string second_name,string login,string password)
        {
            var result = from data in xDocumentPassword.Element("PassWords").Elements("Account")
                         select new
                         {
                             Login = data.Element("Login").Value,
                             Password = data.Element("PassWord").Value,
                             Name = data.Element("First_Name").Value,
                             Second_Name = data.Element("Second_Name").Value
                         };
            foreach (var item in result)
            {
                if (item.Login == login)
                {
                    return false;
                }
            }
            var tempXElementNewAccount = PasswordXML.SetXElement(first_name, second_name, login, password);
            xDocumentPassword.Element("PassWords").Add(tempXElementNewAccount);
            xDocumentPassword.Save("parkingDatePassword.xml");
            return true;
        }

        //Переход на страницу регестрации
        private void ButtonRegestration_Click(object sender, RoutedEventArgs e)
        {
            AdminPage adminPage = new AdminPage();
            Nullable<bool> result = adminPage.ShowDialog();
            if (result == true)
            {
                WindowRegestration windowRegestration = new WindowRegestration();
                if (windowRegestration.ShowDialog() == true)
                {
                    if (Regestration(windowRegestration.FirstName, windowRegestration.SecondName,
                        windowRegestration.Login, windowRegestration.PassWord))
                        MessageBox.Show("Регестрация успешна.");
                    else
                        MessageBox.Show("Логин уже существует.");
                }
                else
                    MessageBox.Show("Ошибка регестрации.");
            }
            else
            {
                MessageBox.Show("Введен не правильный пароль и/или логин дл входа ,как админ","Ошибка входа");
            }
        }

        //Деавторизация 
        private void ButtonLog_Out_Click(object sender, RoutedEventArgs e)
        {
            Accept = false;
            xDocumentPassword.Element("PassWords").Attribute("Accept").Value = false.ToString();
            PasswordXML.SaveXMLPassword(xDocumentPassword);
            MessageBox.Show("Операция успешна.");
        }

        //Открыти диалогового окна об авторских прав
        private void ButtonOwnerShip_Click(object sender, RoutedEventArgs e)
        {
            OwnershipOfIntellectulProperty ownershipOfIntellectulProperty = new OwnershipOfIntellectulProperty();
            ownershipOfIntellectulProperty.ShowDialog();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Window_Instruction window_Instruction = new Window_Instruction();
            window_Instruction.ShowDialog();
        }
    }
}
