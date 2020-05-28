<Window x:Class="Course_Lena.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
        xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
        xmlns:local="clr-namespace:Course_Lena"
        mc:Ignorable="d"
        Title="MainWindow" Height="500" Width="900">
    <Grid>
        <Frame Source="MainPage.xaml"/>
    </Grid>
</Window>

using System.Windows;

namespace Course_Lena
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
    }
}
