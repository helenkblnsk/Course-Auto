using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
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
    /// Interaction logic for FilterData.xaml
    /// </summary>
    public partial class FilterData : Page
    {
        private static XDocument xDocument;
        private static Parking parking;

        public FilterData()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            xDocument = XMLObjectModel.GetXDocument();
            parking = new Parking(Parking.GetParkCards(xDocument), xDocument);
            if (xDocument != null)
            {
                foreach (ParkCard parkCard in parking)
                {
                    listBoxProducts.Items.Add(parkCard.ID + '-' + parkCard.SecondName + '.' + parkCard.FirstName[0]);
                }
            }
        }

        private void ButtonResultSecondName_Click(object sender, RoutedEventArgs e)
        {
            LookingForSecondName();
        }

        private void ButtonResultTrueorFalse_Click(object sender, RoutedEventArgs e)
        {
            LookingForTrue_False();
        }

        private void Button3_Click(object sender, RoutedEventArgs e)
        {
            LookingForCarNum();
        }

        private void Button4_Click(object sender, RoutedEventArgs e)
        {
            LookingForCarModel();
        }
        private void LookingForSecondName()
        {
            StringBuilder stringBuilder = new StringBuilder();
            var tempSecondName = textboxSecondName.Text;
            var result = from data in parking.ParkCards where data.SecondName.ToLower() == tempSecondName.ToLower() select data;
            foreach (ParkCard item in result)
                stringBuilder.Append(item.FileString() + "\n");

            if (stringBuilder.ToString() == "")
                MessageBox.Show("None", "Result");
            else
                MessageBox.Show(stringBuilder.ToString(), "Result");
        }

        private void LookingForTrue_False()
        {
            StringBuilder stringBuilder = new StringBuilder();
            var tempParametr = comboBoxM.Text;
            var result = from data in parking.ParkCards where data.PaidOrNo == bool.Parse(tempParametr) select data;
            foreach (ParkCard item in result)
                stringBuilder.Append(item.FileString() + "\n");

            if (stringBuilder.ToString() == "")
                MessageBox.Show("None", "Result");
            else
                MessageBox.Show(stringBuilder.ToString(), "Result");
        }

        private void LookingForCarNum()
        {
            StringBuilder stringBuilder = new StringBuilder();
            var tempParametr = textBoxW1.Text + textBoxW2.Text + textBoxW3.Text;

            string pattern = @"[AKBCEHIMOPTX]{2}\d{4,5}[AKBCEHIMOPTX]{2}";
            Regex regex = new Regex(pattern);
            if (!regex.IsMatch(tempParametr))
            {
                textBoxW1.Foreground = Brushes.Red;
                textBoxW2.Foreground = Brushes.Red;
                textBoxW3.Foreground = Brushes.Red;
                MessageBox.Show("Неверный формат вводимых данных в поля \"Номерной знак\"");
                return;
            }

            var result = from data in parking.ParkCards where data.CarNum == tempParametr select data;
            foreach (ParkCard item in result)
                stringBuilder.Append(item.FileString() + "\n");

            if (stringBuilder.ToString() == "")
                MessageBox.Show("None", "Result");
            else
                MessageBox.Show(stringBuilder.ToString(), "Result");
        }

        private void LookingForCarModel()
        {
            StringBuilder stringBuilder = new StringBuilder();
            var tempModelCar = textBoxP.Text;
            var result = from data in parking.ParkCards where data.CarModel == tempModelCar select data;
            foreach (ParkCard item in result)
                stringBuilder.Append(item.FileString() + "\n");

            if (stringBuilder.ToString() == "")
                MessageBox.Show("None", "Result");
            else
                MessageBox.Show(stringBuilder.ToString(), "Result");
        }

        private void ListBoxProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string getInf = listBoxProducts.SelectedItem.ToString();
            string pattern = @"\d{16}-\w{2,20}.\w{1}";
            Regex regex = new Regex(pattern);
            MatchCollection matches = null;
            MatchCollection matches1 = null;
            MatchCollection matches2 = null;
            if (regex.IsMatch(getInf))
            {
                matches = Regex.Matches(getInf, @"(\d{16})");
                matches1 = Regex.Matches(getInf, @"(\D{2,20})");
                matches2 = Regex.Matches(getInf, @".(\w{1})$");
            }
            if (matches == null && matches1 == null && matches2 == null)
                return;
            ParkCard temp = null;
            foreach (ParkCard item in parking)
            {
                if (item.ID == matches[0].Value.ToString() && item.SecondName == matches1[0].Value.ToString().Remove(0, 1).Remove(matches1[0].Value.Length - 3, 2) && item.FirstName.StartsWith(matches2[0].Value.ToString().Remove(0, 1)))
                {
                    temp = item;
                    break;
                }
            }
            textBlockInfo.Text = temp.FileString();
        }
    }
}
