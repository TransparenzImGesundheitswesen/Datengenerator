using Datengenerator.Loggen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Datengenerator.Kern
{
    class Datei
    {
        private string Satzartname;
        private string Zeichensatz;
        private string Feldtrennzeichen;
        private string Zeilentrennzeichen;
        private string Endung;
        private XElement SatzartXml;
        private List<string> Primärschlüsselfelder;
        private Dictionary<string, int> Primärschlüssel = new Dictionary<string, int>();

        public Datei(XElement satzartXml)
        {
            SatzartXml = satzartXml;

            Feldtrennzeichen = "#";
            Zeilentrennzeichen = "\r\n";
            Endung = "csv";
            Satzartname = SatzartXml.Attribute("Name").Value;

            if (SatzartXml.Descendants("Primärschlüssel").Descendants("Feld").Any())
                Primärschlüsselfelder = SatzartXml.Descendants("Primärschlüssel").Descendants("Feld").Select(m => m.Value).ToList();
        }

        public void Generieren(int zeilenzahl, int schlechtdatenWahrscheinlichkeit)
        {
            Random r = new Random();
            RandomProportional rp = new RandomProportional();

            using (System.IO.StreamWriter datei = new System.IO.StreamWriter(string.Format("{0}_.{1}", Satzartname, Endung).ZeitstempelAnhängen(), true, Encoding.GetEncoding("iso-8859-15")))
            {
                for (int i = 0; i < zeilenzahl; i++)
                {
                    string primärschlüssel = "";
                    Zeile zeile;
                    string zeileString;

                    do
                    {
                        zeile = new Zeile(Feldtrennzeichen, Zeilentrennzeichen, SatzartXml.Element("Felder"), r, rp);
                        zeileString = zeile.Generieren(schlechtdatenWahrscheinlichkeit);
                        primärschlüssel = "";

                        if (Primärschlüsselfelder != null)
                            foreach (string feld in Primärschlüsselfelder)
                                primärschlüssel += zeile.Feldliste[feld];
                    } while (Primärschlüsselfelder != null && Primärschlüssel.Keys.Contains(primärschlüssel) && !(schlechtdatenWahrscheinlichkeit != 0 && r.Next(0, schlechtdatenWahrscheinlichkeit) == 0));

                    if (Primärschlüsselfelder != null)
                        Primärschlüssel.Add(primärschlüssel, i);

                    datei.Write(zeileString);
                }
            }
        }
    }
}
