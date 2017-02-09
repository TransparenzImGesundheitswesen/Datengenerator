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

            foreach (XElement feldXml in FelderXml.Elements("Feld"))
            {
                Feld feld = new Feld(feldXml, Random);

                if (feldXml.Elements("Konstant").Any())
                    feld = new FeldKonstant(feldXml, Random);
                else if (feldXml.Elements("ZulässigeWerte").Any())
                    feld = new FeldEnum(feldXml, Random);
                else if (feldXml.Attributes("Typ").Any())
                {
                    if (feldXml.Attributes("Typ").First().Value == "Hash")
                        feld = new FeldHash(feldXml, Random);
                    else if (feldXml.Attributes("Typ").First().Value == "IK")
                        feld = new FeldIK(feldXml, Random);
                    else if (feldXml.Attributes("Typ").First().Value == "KV")
                        feld = new FeldKV(feldXml, Random);
                }

                zeile += feld.Generieren() + Feldtrennzeichen;
            }

            zeile = zeile.Substring(0, zeile.Length - 1) + Zeilentrennzeichen;

            return zeile;
        }
    }
}
