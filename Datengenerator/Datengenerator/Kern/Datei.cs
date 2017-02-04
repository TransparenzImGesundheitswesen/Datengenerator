using Datengenerator.Loggen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Datengenerator.Kern
{
    class Datei
    {
        public string Satzartname;
        public string Zeichensatz;
        public string Feldtrennzeichen;
        public string Zeilentrennzeichen;
        public string Endung;
        public XElement SatzartXml;

        public Datei(XElement satzartXml)
        {
            SatzartXml = satzartXml;

            Feldtrennzeichen = "#";
            Zeilentrennzeichen = "\r\n";
            Endung = "csv";
            Satzartname = SatzartXml.Attribute("Name").Value;
        }

        public void Generieren(int zeilenzahl)
        {
            Random r = new Random();

            using (System.IO.StreamWriter datei = new System.IO.StreamWriter(string.Format("{0}_.{1}", Satzartname, Endung).ZeitstempelAnhängen(), true))
            {
                for (int i = 0; i < zeilenzahl; i++)
                {
                    datei.Write(new Zeile(Feldtrennzeichen, Zeilentrennzeichen, SatzartXml.Element("Felder"), r).Generieren());
                }
            }
        }
    }
}
