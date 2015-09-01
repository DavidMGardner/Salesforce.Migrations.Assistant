using System.Xml;
using System.Xml.Linq;


namespace Salesforce.Migrations.Assistant.Library
{
    public static class GeneratePackageFile
    {

        public static void WithXMLOutput()
        {
            XmlOutput xo = new XmlOutput()
                .XmlDeclaration()
                .Node("package").Attribute("xmlns", "http://soap.sforce.com/2006/04/metadata").Within();
                for (int i = 0; i < 10; i++ )
                {
                    xo.Node("types").Within()
                        .Node("members").InnerText("SamplePage")
                        .Node("name").InnerText("ApexPage")
                        .EndWithin();
                };
                
                xo.EndWithin()
                .Node("version").InnerText("28.0");

            xo.GetXmlDocument().Save("D:\\document.xml");
        }

        public static void Go()
        {
            //var ns = XNamespace.Get("http://soap.sforce.com/2006/04/metadata");

            XmlDocument doc = new XmlDocument();

            //(1) the xml declaration is recommended, but not mandatory
            XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
          
            XmlElement root = doc.DocumentElement;
            doc.InsertBefore(xmlDeclaration, root);

            string ns = "http://soap.sforce.com/2006/04/metadata";

            //(2) string.Empty makes cleaner code
            XmlElement element1 = doc.CreateElement(string.Empty, "package", ns);
            doc.AppendChild(element1);

            XmlElement element2 = doc.CreateElement(string.Empty, "types", ns);
            element1.AppendChild(element2);

            XmlElement element3 = doc.CreateElement(string.Empty, "members", ns);
            XmlText text1 = doc.CreateTextNode("text");
            element3.AppendChild(text1);
            element2.AppendChild(element3);

            XmlElement element4 = doc.CreateElement(string.Empty, "name", ns);
            XmlText text2 = doc.CreateTextNode("other text");

            XmlElement version = doc.CreateElement(string.Empty, "version", ns);
            XmlText versionText = doc.CreateTextNode("28.0");
            version.AppendChild(versionText);
            element1.AppendChild(version);
            

            element4.AppendChild(text2);
            element2.AppendChild(element4);

            doc.Save("D:\\document.xml");
        } 
    }
}