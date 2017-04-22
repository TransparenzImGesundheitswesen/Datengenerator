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
        public readonly RandomProportional RandomProp;

        public Zeile(string feldtrennzeichen, string zeilentrennzeichen, XElement felderXml, Random r, RandomProportional rp)
        {
            Feldtrennzeichen = feldtrennzeichen;
            Zeilentrennzeichen = zeilentrennzeichen;
            FelderXml = felderXml;
            Random = r;
            RandomProp = rp;
        }

        public string Generieren(int schlechtdatenWahrscheinlichkeit)
        {
            string zeile = "";
            Dictionary<string, string> feldliste = new Dictionary<string, string>();

            foreach (XElement feldXml in FelderXml.Elements("Feld"))
            {
                Feld feld = new Feld(feldXml, Random, schlechtdatenWahrscheinlichkeit);

                if (
                    (feld.Art == Feldart.m // Bedingtes Muss-Feld
                        && feldliste[feldXml.Descendants("BedingungFeld").Select(m => m.Value).First()] != feldXml.Descendants("BedingungWert").Select(m => m.Value).First())
                    ||
                    (feld.Art == Feldart.K // Kann-Feld
                        && Random.Next(0, 2) == 0)
                    ||
                    (feld.Art == Feldart.M && schlechtdatenWahrscheinlichkeit > 0 // Muss-Feld und Schlechtdaten aktiviert
                        && Random.Next(0, schlechtdatenWahrscheinlichkeit) == 0)
                   )
                {
                    zeile += "" + Feldtrennzeichen;
                    feldliste.Add(feld.Nummer, "");
                }
                else
                {

                    if (feldXml.Elements("Konstant").Any())
                        feld = new FeldKonstant(feldXml, Random);
                    else if (feldXml.Elements("ZulässigeWerte").Any())
                        feld = new FeldEnum(feldXml, Random, schlechtdatenWahrscheinlichkeit);
                    else if (feldXml.Attributes("Typ").Any())
                    {
                        if (feldXml.Attributes("Typ").First().Value == "Hash")
                            feld = new FeldHash(feldXml, Random, schlechtdatenWahrscheinlichkeit);
                        else if (feldXml.Attributes("Typ").First().Value == "IK")
                            feld = new FeldIK(feldXml, Random, schlechtdatenWahrscheinlichkeit);
                        else if (feldXml.Attributes("Typ").First().Value == "KV")
                            feld = new FeldKV(feldXml, Random);
                        else if (feldXml.Attributes("Typ").First().Value == "PLZ")
                            feld = new FeldPLZ(feldXml, Random);
                        else if (feldXml.Attributes("Typ").First().Value == "KalTag")
                            feld = new FeldKalTag(feldXml, Random);
                        else if (feldXml.Attributes("Typ").First().Value == "VsTage")
                            feld = new FeldVsTage(feldXml, RandomProp);
                        else if (feldXml.Attributes("Typ").First().Value == "ICD")
                            feld = new FeldICD(feldXml, Random, schlechtdatenWahrscheinlichkeit);
                        else if (feldXml.Attributes("Typ").First().Value == "FGCode")
                            feld = new FeldFGCode(feldXml, Random, schlechtdatenWahrscheinlichkeit);
                        else if (feldXml.Attributes("Typ").First().Value == "GOP")
                            feld = new FeldGOP(feldXml, Random, schlechtdatenWahrscheinlichkeit);
                        else if (feldXml.Attributes("Typ").First().Value == "Ganzzahl")
                            feld = new FeldGanzzahl(feldXml, Random, schlechtdatenWahrscheinlichkeit);
                        else if (feldXml.Attributes("Typ").First().Value == "GebrZahl")
                            feld = new FeldGebrZahl(feldXml, Random, schlechtdatenWahrscheinlichkeit);
                    }

                    string wert = feld.Generieren();
                    feldliste.Add(feld.Nummer, wert);
                    zeile += wert + Feldtrennzeichen;
                }

            }

            zeile = zeile.Substring(0, zeile.Length - 1) + Zeilentrennzeichen;

            return zeile;
        }
    }
}
