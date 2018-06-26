using System;
using System.Threading.Tasks;
using System.Linq;
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

            Konfiguration.Zeichensatz = xml.Descendants("Dateikonventionen").Descendants("Zeichensatz").Select(m => m.Value).First();
            Konfiguration.Dateiname = xml.Descendants("Dateikonventionen").Descendants("Dateiname").Select(m => m.Value).First();

            if (xml.Descendants("Dateikonventionen").Descendants("Auffüllen").Any())
            {
                foreach (XElement auffüllenXml in xml.Descendants("Dateikonventionen").Elements("Auffüllen"))
                {
                    DateiattributAuffüllen auffüllen = new DateiattributAuffüllen
                    {
                        Attribut = auffüllenXml.Element("Attribut").Value,
                        Länge = int.Parse(auffüllenXml.Element("Länge").Value),
                        Zeichen = auffüllenXml.Element("Zeichen").Value
                    };

                    Konfiguration.Auffüllen.Add(auffüllen);
                }
            }

            string feldtrennzeichen = xml.Descendants("Dateikonventionen").Descendants("Feldtrennzeichen").Select(m => m.Value).First();
            if (feldtrennzeichen.StartsWith("0x"))
                foreach (string zeichen in feldtrennzeichen.Split(' '))
                    Konfiguration.Feldtrennzeichen += System.Convert.ToChar(System.Convert.ToUInt32(feldtrennzeichen, 16)).ToString();
            else
                Konfiguration.Feldtrennzeichen = feldtrennzeichen;

            string zeilentrennzeichen = xml.Descendants("Dateikonventionen").Descendants("Zeilentrennzeichen").Select(m => m.Value).First();
            if (zeilentrennzeichen.StartsWith("0x"))
                foreach (string zeichen in zeilentrennzeichen.Split(' '))
                    Konfiguration.Zeilentrennzeichen += System.Convert.ToChar(System.Convert.ToUInt32(zeichen, 16)).ToString();
            else
                Konfiguration.Zeilentrennzeichen = zeilentrennzeichen;

            //int rsn = 2000;
            foreach (Dictionary<string, string> dateiattribute in Konfiguration.DateiattributeKombinationen)
            //Parallel.ForEach(Konfiguration.DateiattributeKombinationen, dateiattribute =>
            {
                // Dieser Code geht davon aus, dass in der XML-Datei die Satzarten mit der Fremdschlüsselbeziehung
                // immer nach der Satzart steht, auf die die Beziehung zeigt, damit die Event-Registrierung klappt    
                IEnumerable<XElement> satzartenXml = xml.Descendants("Satzarten");
                List<Datei> alleDateien = new List<Datei>();

                foreach (XElement satzartXml in satzartenXml.Elements("Satzart"))
                {
                    Datei datei = new Datei(satzartXml, alleDateien, dateiattribute, (Konfiguration.RSN > 0 ? Konfiguration.RSN++.ToString() : null));
                    alleDateien.Add(datei);
                }

                foreach (Datei datei in alleDateien.Where(m => !m.HatFremdschlüssel))
                {
                    datei.Generieren();
                }
            }

            Logger.SchlechtfelderSpeichern();

            Console.WriteLine("Narf!");
            Console.ReadLine();
        }
    }
}
