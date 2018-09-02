using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StreamXMLFragments
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static string CurrentProgramPath = System.Environment.CurrentDirectory.Replace("\\bin\\Debug", "\\");
        private string Configfilename = CurrentProgramPath + "Files\\CustomerItems.xml";
        private string Configfilename2 = CurrentProgramPath + "Files\\PurchaseOrders.xml";
        
        static IEnumerable<XElement> StreamCustomerItem(string uri)
        {
            using (XmlReader reader = XmlReader.Create(uri))
            {
                XElement name = null;
                XElement item = null;

                reader.MoveToContent();

                // Parse the file, save header information when encountered, and yield the  
                // Item XElement objects as they are created.  

                // loop through Customer elements  
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element
                        && reader.Name == "Customer")
                    {
                        // move to Name element  
                        while (reader.Read())
                        {
                            if (reader.NodeType == XmlNodeType.Element &&
                                reader.Name == "Name")
                            {
                                name = XElement.ReadFrom(reader) as XElement;
                                break;
                            }
                        }

                        // loop through Item elements  
                        while (reader.Read())
                        {
                            if (reader.NodeType == XmlNodeType.EndElement) // Parse the end </Customer> tag
                                break;
                            if (reader.NodeType == XmlNodeType.Element
                                && reader.Name == "Item")
                            {
                                item = XElement.ReadFrom(reader) as XElement;
                                if (item != null)
                                {
                                    XElement tempRoot = new XElement("Root",   // Construct this element to add item as a child
                                        new XElement(name)                     // so that TransformXML() can use it to reverse tree  
                                    );
                                    tempRoot.Add(item);                  
                                    yield return item;
                                }
                            }
                        }
                    }
                }
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            TransformXML();            
        }

        private void TransformXML()
        {
            //XElement xmlTree = new XElement("Root",
            //from el in StreamCustomerItem(Configfilename)
            //where (int)el.Element("Key") >= 3 && (int)el.Element("Key") <= 7
            //select new XElement("Item",
            //    new XElement("Customer", (string)el.Parent.Element("Name")),  // here, Parent is the "root" node constructed in StreamCustomerItem()
            //    new XElement(el.Element("Key"))
            //)
            //);
            ////Console .WriteLine(xmlTree);
            //XMLDataTxtBx.Text = xmlTree.ToString();



            //XElement purchaseOrders = XElement.Load(Configfilename2);

            //IEnumerable<XObject> subset =
            //    from xobj in purchaseOrders.Find("1999")
            //    select xobj;

            //foreach (XObject obj in subset)
            //{
            //    XMLDataTxtBx.Text += obj.GetXPath() + "\n";
            //    //Console.WriteLine(obj.GetXPath());
            //    if (obj.GetType() == typeof(XElement))
            //        XMLDataTxtBx.Text += ((XElement)obj).Value + "\n";
            //        //Console.WriteLine(((XElement)obj).Value);
            //    else if (obj.GetType() == typeof(XAttribute))
            //        XMLDataTxtBx.Text += ((XAttribute)obj).Value + "\n";
            //        //Console.WriteLine(((XAttribute)obj).Value);
            //}

            //var bench = new XElement("bench",
            //                new XElement("toolbox",
            //                    new XElement("handtool", "Hammer"),
            //                    new XElement("handtool", "Rasp")
            //                    ),
            //                new XElement("toolbox",
            //                    new XElement("handtool", "Saw"),
            //                    new XElement("powertool", "Nailgun")
            //                    ),
            //                new XComment("Be carefull with the nailgun")
            //             );

            //foreach(var node in bench.Nodes())
            //{
            //    XMLDataTxtBx.Text += node.ToString(SaveOptions.DisableFormatting) + ".\n";
            //}

            //foreach(var element in bench.Elements())
            //{
            //    XMLDataTxtBx.Text += element.Name + "=" + element.Value + "\n";
            //}

            XElement settings = new XElement("settings",
                                    new XElement("timeout", 30)
                                );
            XMLDataTxtBx.Text += settings.ToString() + "\n";

            settings.SetValue("blah");
            XMLDataTxtBx.Text += settings.ToString() + "\n";

            settings.SetElementValue("timeout", 30);
            settings.Add(new XElement("timeout", 60));
            XMLDataTxtBx.Text += settings.ToString(SaveOptions.DisableFormatting) + "\n";

        }
    } 

}
