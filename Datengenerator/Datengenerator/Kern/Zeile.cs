using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Datengenerator.Kern
{
    class Zeile
    {
        public string Feldtrennzeichen;
        public string Zeilentrennzeichen;
        public XElement FelderXml;
        public readonly Random Random;

        public Zeile(string feldtrennzeichen, string zeilentrennzeichen, XElement felderXml, Random r)
        {
            Feldtrennzeichen = feldtrennzeichen;
            Zeilentrennzeichen = zeilentrennzeichen;
            FelderXml = felderXml;
            Random = r;
        }

        public string Generieren()
        {
            string zeile = "";
            Feld feld;

            foreach (XElement feldXml in FelderXml.Elements("Feld"))
            {
                if (feldXml.Elements("Konstant").Any())
                    feld = new FeldKonstant(feldXml, Random);
                if (feldXml.Elements("ZulässigeWerte").Any())
                    feld = new FeldEnum(feldXml, Random);
                else
                    feld = new Feld(feldXml, Random);

                zeile += feld.Generieren() + Feldtrennzeichen;
            }

            zeile = zeile.Substring(0, zeile.Length - 1) + Zeilentrennzeichen;

            return zeile;
        }
    }
}
