using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml.Linq;

namespace Course_Lena
{
    /// <summary>
    /// Interaction logic for AddData.xaml
    /// </summary>
    public partial class AddData : Page
    {
        private static XDocument xDocument = XMLObjectModel.GetXDocument();
        private Parking parking = new Parking(xDocument);


        public AddData()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                DatagridTableUpDate();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        #region MethodOfButtons

        private void DisChargeButton_Click(object sender, RoutedEventArgs e)
        {
            listboxPropducts.SelectedIndex = -1;
        }

        private void UpDateButton_Click(object sender, RoutedEventArgs e)
        {
            UpDateMethod();
        }

        private void SaveXData_Click(object sender, RoutedEventArgs e)
        {
            SaveXDocument();
            SaveTXTFile();
            SaveXMLFile();
        }


        private void RemButton_Click(object sender, RoutedEventArgs e)
        {
            RemoveParkCard();
            SaveXDocument();
            UpDateParkCards();
            UpDateListBoxProducts();
            DatagridTableUpDate();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddButtonMethof();
        }
        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            LoadDataBase();
        }


        #endregion

        #region MainMethods
        //Сохрание текстового файла
        private void SaveTXTFile()
        {
            SaveFileDialog saveTXTFile = new SaveFileDialog
            {
                Filter = "Text Files | *.txt",
                DefaultExt = "txt"
            };
            Nullable<bool> resultTXT = saveTXTFile.ShowDialog();
            if (resultTXT == false) return;
            FileStream fileStream = new FileStream(saveTXTFile.FileName, FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter streamWriter = new StreamWriter(fileStream);
            foreach (var item in parking.ParkCards)
            {
                streamWriter.Write(item.ToString());
                streamWriter.Write("\n");
            }
            streamWriter.Close();
            fileStream.Close();
        }

        //Сохрание XML-файла
        private void SaveXMLFile()
        {
            SaveFileDialog saveXMLFile = new SaveFileDialog
            {
                Filter = "XML FIle | *.xml",
                DefaultExt = "xml"
            };
            Nullable<bool> resultXML = saveXMLFile.ShowDialog();
            if (resultXML == false) return;
            if (!xDocument.Element("Parking").HasAttributes)
                xDocument.Element("Parking").Add(new XAttribute("IDCode", "bjhdchbjd"));
            new XDocument(xDocument).Save(saveXMLFile.FileName);
        }

        //Загрузка БД из внешнего источника
        private void LoadDataBase()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            Nullable<bool> result = openFileDialog.ShowDialog();
            if (result == false)
                return;
            try
            {
                xDocument = XDocument.Load(openFileDialog.FileName);
                if (xDocument.Root.Name != "Parking")
                    if (xDocument.Element("Parking").HasAttributes)
                        if (xDocument.Element("Parking").FirstAttribute.Name == "IDCode")
                            if (xDocument.Element("Parking").Attribute("IDCode").Value != "bjhdchbjd")
                                throw new Exception("Ошибка загружаемого файла");
                SaveXDocument();
                UpDateListBoxProducts();
                UpDateParkCards();
                DatagridTableUpDate();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        //Обновление записи
        private void UpDateMethod()
        {
            bool result = false;
            try { result = TestNull(); } catch (Exception exception) { MessageBox.Show(exception.Message); }
            if (!result)
                return;
            XElement selProduct = (XElement)listboxPropducts.SelectedItem;
            XElement newPropduct = ParkCard.GetXElement(IDTextBox, NumberTextBox, First_NameTextBox, Second_NameTextBox, Paid_Or_NoComboBox, Car_ModelTextBox, Car_NumTextBox);
            //if (selProduct != null)
            try { selProduct.Remove(); } catch (NullReferenceException nullexception) { MessageBox.Show(nullexception.Message); }
            SaveXDocument();
            AddNewXElemetInXMLDocument(newPropduct);
            UpDateListBoxProducts();
            DatagridTableUpDate();
            UpDateParkCards();
        }

        //Обновление таблицы данных
        private void DatagridTableUpDate()
        {
            listboxPropducts.DataContext = xDocument.Descendants("Parking").Elements();
            var apartms = from a in xDocument.Descendants("IDParkCard")
                          select a;
            Grid.ItemsSource = apartms.ToList();
        }
        //Метод удаление выбранной записи
        private void RemoveParkCard()
        {
            int index = listboxPropducts.SelectedIndex;
            if (index < 0) return;
            XElement selProduct = (XElement)listboxPropducts.SelectedItem;
            selProduct.Remove();
        }

        //Метод обновления списка записей
        private void UpDateListBoxProducts()
        {
            listboxPropducts.DataContext = xDocument.Descendants("Parking").Elements();
        }

        //Метод обновления системного масива из элементов ParkCard
        private void UpDateParkCards()
        {
            parking = new Parking(xDocument);
        }

        //Метод сохранения XML-document
        private void SaveXDocument()
        {
            xDocument.Save("parkingDate.xml");
        }

        //Метод добавления новой записи
        private void AddButtonMethof()
        {
            BlackFontTextBoxes();//проверка на отсутствие ошибок в вводе
            bool result;
            if (result = !TestNull()) return;//проверка на нулевые строки в textboxes

            result = false;
            ParkCard temp = null;

            result = CheckAllTextBox(ref temp);//проверка на соответсвие с другими записями существующей БД 
            if (!result) return;

            ParkCard tempParkcard = null;
            if (temp != null)
                tempParkcard = new ParkCard(temp.ID, parking.GetFreeParkPLace(), First_NameTextBox.Text, Second_NameTextBox.Text, bool.Parse(Paid_Or_NoComboBox.Text), Car_ModelTextBox.Text, Car_NumTextBox.Text);
            else
                tempParkcard = new ParkCard(ParkCard.RandomID(this.parking), parking.GetFreeParkPLace(), First_NameTextBox.Text, Second_NameTextBox.Text, bool.Parse(Paid_Or_NoComboBox.Text), Car_ModelTextBox.Text, Car_NumTextBox.Text);

            NumberTextBox.Text = temp.ParkPlace.ToString();
            IDTextBox.Text = tempParkcard.ID.ToString();

            XElement newPropduct = ParkCard.GetXElement(IDTextBox, NumberTextBox, First_NameTextBox, Second_NameTextBox, Paid_Or_NoComboBox, Car_ModelTextBox, Car_NumTextBox);

            AddNewXElemetInXMLDocument(newPropduct);
            SaveXDocument();
            UpDateListBoxProducts();
            UpDateParkCards();
            DatagridTableUpDate();
        }

        //Метод добавления нового узла в XMl-document
        private void AddNewXElemetInXMLDocument(XElement newPropduct)
        {
            xDocument.Element("Parking").Add(newPropduct);
        }
        #endregion

        #region Checked
        //Проверка на нулевые строки
        private bool TestNull()
        {
            try
            {
                if (Second_NameTextBox.Text == "" || Paid_Or_NoComboBox.Text == ""
                    || Car_ModelTextBox.Text == "" || Car_NumTextBox.Text == "")
                {
                    throw new Exception("Поле для ввода данных пустое пустое");
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, exception.Source);
                return false;
            }
            return true;
        }
        //Проверка на соответсвие с другими записями существующей БД 
        private bool CheckAllTextBox(ref ParkCard parkCard)
        {
            try
            {
                AcceptFirstAndSecondNAme(First_NameTextBox.Text, Second_NameTextBox.Text);
                AcceptNumAuto(Car_NumTextBox.Text);
                for (int j = 0; j < parking.ParkCards.Length; j++)
                {

                    if (Car_NumTextBox.Text == parking.ParkCards[j].CarNum)
                    {
                        Car_NumTextBox.Foreground = Brushes.Red;
                        throw new Exception("Car is in the base");
                    }

                    if (First_NameTextBox.Text == parking.ParkCards[j].FirstName && Second_NameTextBox.Text == parking.ParkCards[j].SecondName)
                    {
                        parkCard = parking.ParkCards[j];
                    }

                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                return false;
            }
            return true;
        }
        private void BlackFontTextBoxes()
        {
            TextBox[] textBoxes = { IDTextBox, First_NameTextBox, Second_NameTextBox, Car_NumTextBox };
            for (int i = 0; i < textBoxes.Length; i++)
            {
                textBoxes[i].Foreground = Brushes.Black;
            }
        }
        #endregion

        #region AcceptingFields(Regular Expressions)
        //private bool AcceptId(ref string IDstring)//Проверка на правильность введенных данных ID код
        //{
        //    string pattern = @"\d{0,16}";
        //    Regex regex = new Regex(pattern);
        //    if (!regex.IsMatch(IDstring))
        //        throw new FormatException("ID имеет не корректный формат");
        //    if (IDstring.Length != 16)
        //    {
        //        IDstring = IDstring.Insert(0, new string('0', 16 - IDstring.Length));
        //    }
        //    return true;
        //}

        private bool AcceptFirstAndSecondNAme(string FirstString, string secondString)//Проверка на правильность введенных данных (имя ,фамилия)
        {
            string pattern = @"[a-z]{2,20}$";
            Regex regex = new Regex(pattern);
            if (
               !(regex.IsMatch(FirstString)
               && regex.IsMatch(secondString))
                )
            {
                First_NameTextBox.Foreground = Brushes.Red;
                Second_NameTextBox.Foreground = Brushes.Red;
                throw new FormatException("Не верный формат в поле(-ях) имени и/или фамилии.");
            }
            return true;
        }
        public bool AcceptNumAuto(string NomOfCar)//Проверка на правильность введенных данных (номерной знак автомобиля)
        {
            string pattern = @"[AKBCEHIMOPTX]{2}\d{4,5}[AKBCEHIMOPTX]{2}";
            Regex regex = new Regex(pattern);
            if (!regex.IsMatch(NomOfCar))
            {
                Car_NumTextBox.Foreground = Brushes.Red;
                throw new FormatException("Не верный формат вводимых данных в полt номерных знаков.");
            }
            return true;
        }


        #endregion

        private void buttonFreePark_Click(object sender, RoutedEventArgs e)
        {
            string resultSTR;
            var resultINT = parking.GetCountFreePlace(out resultSTR);
            MessageBox.Show(resultSTR);
        }
    }
}
