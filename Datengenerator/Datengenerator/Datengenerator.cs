using System;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Collections.Generic;

using Datengenerator.Kern;
using Datengenerator.Konfig;
using Datengenerator.Loggen;
using Datengenerator.XML;

namespace Datengenerator
{
    class Datengenerator
    {
        static void Main(string[] args)
        {
            foreach (string xsd in Konfiguration.Xsd)
                Logger.Loggen(string.Format("XSD-Datei: {0}", xsd));
            Logger.Loggen(string.Format("XML-Datei: {0}", Konfiguration.Xml));
            Logger.Loggen(string.Format("Validieren: {0}", Konfiguration.Validieren ? "ja" : "nein"));

            if (Konfiguration.Validieren)
            {
                var validierer = new XsdValidierer();

                foreach (string xsd in Konfiguration.Xsd)
                    validierer.SchemaHinzufügen(xsd);
                var istValide = validierer.IstValide(Konfiguration.Xml);

                Console.WriteLine(istValide);

                Logger.Loggen(string.Format("Ist valide: {0}", istValide ? "ja" : "nein"));
            }

            XElement xml = XElement.Load(Konfiguration.Xml);

            IEnumerable<XElement> schlüsselverzeichnisseXml = xml.Descendants("Schlüsselverzeichnisse");
            Schlüsselverzeichnismanager.SchlüsselverzeichnisseHinzufügen(schlüsselverzeichnisseXml);

            IEnumerable<XElement> satzartenXml = xml.Descendants("Satzarten");
            //foreach (XElement satzartXml in satzartenXml.Elements("Satzart"))
            Parallel.ForEach(satzartenXml.Elements("Satzart"), satzartXml =>
            {
                Datei datei = new Datei(satzartXml);
                datei.Generieren(Konfiguration.AnzahlZeilen);
            });

            Console.WriteLine("Narf!");
            Console.ReadLine();
        }
    }
}
