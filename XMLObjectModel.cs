using System.IO;
using System.Windows;
using System.Xml.Linq;

namespace Course_Lena
{
    class XMLObjectModel
    {
        static public XDocument GetXDocument()
        {
            try
            {
                if (!File.Exists("parkingDate.xml"))
                {
                    FileInfo fileInfo = new FileInfo("parkingDate.xml");
                    fileInfo.Create();
                    var xdoc = XDocument.Load("parkingDate.xml");
                    xdoc.Add(new XElement("Parking", new XAttribute("IDCode", "bjhdchbjd")));
                    xdoc.Save("parkingDate.xml");
                }
                XDocument xDocument = XDocument.Load("parkingDate.xml");
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
