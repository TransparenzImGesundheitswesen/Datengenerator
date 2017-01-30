using System;

using Datengenerator.XML;
using Datengenerator.Loggen;

namespace Datengenerator
{
    class Datengenerator
    {
        static void Main(string[] args)
        {
            string xsdDatei = "XML-Testdateien/ASV.xsd";
            string xmlDatei = "XML-Testdateien/ASV.xml";

            var validierer = new XsdValidierer();
            validierer.SchemaHinzufügen(xsdDatei);
            var istValide = validierer.IstValide(xmlDatei);
            Console.WriteLine(istValide);

            Logger.Loggen(string.Format("XSD-Datei: {0}", xsdDatei));
            Logger.Loggen(string.Format("XML-Datei: {0}", xmlDatei));
            Logger.Loggen(string.Format("valide: {0}", istValide));

            Console.WriteLine("Narf!");
            Console.ReadLine();
        }
    }
}
