<Window x:Class="Course_Lena.OwnershipOfIntellectulProperty"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
        xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
        xmlns:local="clr-namespace:Course_Lena"
        mc:Ignorable="d"
        Title="Авторские права" Height="250" Width="350">
    <Grid Margin="5">
        <TextBlock Text="Курсовая работа" HorizontalAlignment="Center" Margin="0,10,0,0"></TextBlock>
        <TextBlock Text="с дисциплини" TextWrapping="Wrap"  HorizontalAlignment="Center" Margin="0,30,0,0"></TextBlock>
        <TextBlock Text="«Програмирование и теория алгоритмов»" TextWrapping="Wrap" HorizontalAlignment="Center" Margin="0,50,0,0"></TextBlock>
        <TextBlock Text="на тему: «Создание базы данных на основе .NET»" HorizontalAlignment="Center" Margin="0,70,0,0"></TextBlock>
        <TextBlock Text="Студент 1 курса, групи АТ–193 " TextWrapping="Wrap" VerticalAlignment="Top" HorizontalAlignment="Center" Margin="0,90,0,0"></TextBlock>
        <TextBlock Text="специальности" HorizontalAlignment="Center" Margin="0,110,0,0"></TextBlock>
        <TextBlock Text="«Автоматизация и компьютерно-интегрованые технологии»" Margin="0,130,0,0"></TextBlock>
        <TextBlock Text="ФИО: Кобылянская Е.В." Margin="0,150,0,0" HorizontalAlignment="Center"></TextBlock>
        <TextBlock Text="Руководитель: доц. Сперанський В.О." Margin="0,170,0,0" HorizontalAlignment="Center"></TextBlock>
    </Grid>
</Window>


using System.Windows;

namespace Course_Lena
{
    /// <summary>
    /// Interaction logic for PassWordWindow.xaml
    /// </summary>
    public partial class PassWordWindow : Window
    {
        public PassWordWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        public string PassWord => textBoxPassWord.Password;
        public string Login => textBoxLogin.Text;
    }
}
