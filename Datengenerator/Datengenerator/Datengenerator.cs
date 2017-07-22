using System;
using System.Threading.Tasks;
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;

using Datengenerator.Kern;
using Datengenerator.Konfig;
using Datengenerator.Loggen;
using Datengenerator.XML;
using System.Threading;

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

            // Dieser Code geht davon aus, dass in der XML-Datei die Satzarten mit der Fremdschlüsselbeziehung
            // immer nach der Satzart steht, auf die die Beziehung zeigt, damit die Event-Registrierung klappt            
            List<Datei> alleDateien = new List<Datei>();

            foreach (XElement satzartXml in satzartenXml.Elements("Satzart"))
            {
                Datei datei = new Datei(satzartXml, alleDateien);
                alleDateien.Add(datei);
            }

            foreach(Datei datei in alleDateien.Where(m => !m.HatFremdschlüssel))
            {
                datei.Generieren(Konfiguration.AnzahlZeilen, Konfiguration.SchlechtdatenWahrscheinlichkeit);
            }

            //Parallel.ForEach(alleDateien.Where(m => !m.HatFremdschlüssel), datei =>
            //{
            //    datei.Generieren(Konfiguration.AnzahlZeilen, Konfiguration.SchlechtdatenWahrscheinlichkeit);
            //});
            
            //Thread.Sleep(5000);

            //foreach(Datei datei in alleDateien)
            //{
            //    datei.Schreiben();
            //}

            Console.WriteLine("Narf!");
            Console.ReadLine();
        }
    }
}
