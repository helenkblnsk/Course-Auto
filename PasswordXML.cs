using System.IO;
using System.Windows;
using System.Xml.Linq;

namespace Course_Lena
{
    class PasswordXML
    {
        public static void SaveXMLPassword(XDocument xDocument)
        {
            xDocument.Save("parkingDatePassword.xml");
        }
        static public XElement SetXElement(string firstname, string secondname, string login, string password)
        {
            return new XElement("Account",
                        new XElement("First_Name", firstname),
                        new XElement("Second_Name", secondname),
                        new XElement("Login", login),
                        new XElement("PassWord", password)
                        );
        }
        static public XDocument GetXDocument()
        {
            try
            {
                if (!File.Exists("parkingDatePassword.xml"))
                {
                    XDocument temp = new XDocument();
                    temp.Add(new XElement("PassWords", new XAttribute("Accept", false)));
                    temp.Element("PassWords").Add(new XElement("Account",
                        new XElement("First_Name", "admin"),
                        new XElement("Second_Name", "admin"),
                        new XElement("Login", "admin"),
                        new XElement("PassWord", "admin")
                        ));
                    temp.Save("parkingDatePassword.xml");
                }
                XDocument xDocument = XDocument.Load("parkingDatePassword.xml");
                return xDocument;
            }
            catch (System.IO.FileNotFoundException ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }
    }
}
