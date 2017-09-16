using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Datengenerator.Konfig;
using Datengenerator.Loggen;

namespace Datengenerator.Kern
{
    class Zeile
    {
        public XElement FelderXml;
        public readonly Random Random;
        public readonly RandomProportional RandomProp;
        public Dictionary<string, string> Feldliste = new Dictionary<string, string>();
        public Dictionary<string, string> Dateiattribute = new Dictionary<string, string>();

        public Zeile(XElement felderXml, Random r, RandomProportional rp, Dictionary<string, string> dateiattribute)
        {
            FelderXml = felderXml;
            Random = r;
            RandomProp = rp;
            Dateiattribute = dateiattribute;
        }

        public string Generieren(Dictionary<string, string> fremdschlüssel, string dateiname, int zeilennummer)
        {
            string zeile = "";

            foreach (XElement feldXml in FelderXml.Elements("Feld"))
            {
                Feld feld = new Feld(feldXml, Random);

                if (fremdschlüssel != null && fremdschlüssel.Count > 0 && fremdschlüssel.Keys.Contains(feld.Nummer)
                    && !(Konfiguration.SchlechtdatenWahrscheinlichkeitFremdschlüssel > 0 && Random.Next(0, Konfiguration.SchlechtdatenWahrscheinlichkeitFremdschlüssel) == 0))
                {
                    zeile += fremdschlüssel[feld.Nummer] + Konfiguration.Feldtrennzeichen;
                    Feldliste.Add(feld.Nummer, fremdschlüssel[feld.Nummer]);
                }
                else
                {
                    if (
                        (feld.Art == Feldart.m // Bedingtes Muss-Feld
                            && Feldliste[feldXml.Descendants("BedingungFeld").Select(m => m.Value).First()] != feldXml.Descendants("BedingungWert").Select(m => m.Value).First())
                        ||
                        (feld.Art == Feldart.K // Kann-Feld
                            && Random.Next(0, 2) == 0)                        
                       )
                    {
                        zeile += "" + Konfiguration.Feldtrennzeichen;
                        Feldliste.Add(feld.Nummer, "");
                    }
                    else if (feld.Art == Feldart.M && Konfiguration.SchlechtdatenWahrscheinlichkeit > 0 // Muss-Feld und Schlechtdaten aktiviert
                            && Random.Next(0, Konfiguration.SchlechtdatenWahrscheinlichkeit) == 0)
                    {
                        zeile += "" + Konfiguration.Feldtrennzeichen;
                        Feldliste.Add(feld.Nummer, "");

                        Logger.LoggenSchlechtfeld(new Schlechtfeld { Dateiname = dateiname, Zeile = zeilennummer + 1, Feld = feld.Nummer });
                    }
                    else
                    {
                        if (feldXml.Elements("Konstant").Any())
                            feld = new FeldKonstant(feldXml, Random);
                        else if (feldXml.Elements("ZulässigeWerte").Any())
                            feld = new FeldEnum(feldXml, Random);
                        else if (feldXml.Elements("Format").Any())
                            feld = new FeldQuartalDatum(feldXml, Random, Dateiattribute, Feldliste);
                        else if (feldXml.Attributes("Typ").Any())
                        {
                            if (feldXml.Attributes("Typ").First().Value == "Hash")
                                feld = new FeldHash(feldXml, Random);
                            else if (feldXml.Attributes("Typ").First().Value == "IK")
                                feld = new FeldIK(feldXml, Random, Dateiattribute);
                            else if (feldXml.Attributes("Typ").First().Value == "KV")
                                feld = new FeldKV(feldXml, Random);
                            else if (feldXml.Attributes("Typ").First().Value == "KVVektor")
                                feld = new FeldKVVektor(feldXml, Random);
                            else if (feldXml.Attributes("Typ").First().Value == "PLZ")
                                feld = new FeldPLZ(feldXml, Random);
                            else if (feldXml.Attributes("Typ").First().Value == "KalTag")
                                feld = new FeldKalTag(feldXml, Random);
                            else if (feldXml.Attributes("Typ").First().Value == "VsTage")
                                feld = new FeldVsTage(feldXml, Random, RandomProp);
                            else if (feldXml.Attributes("Typ").First().Value == "ICD")
                                feld = new FeldICD(feldXml, Random);
                            else if (feldXml.Attributes("Typ").First().Value == "FGCode")
                                feld = new FeldFGCode(feldXml, Random);
                            else if (feldXml.Attributes("Typ").First().Value == "GOP")
                                feld = new FeldGOP(feldXml, Random);
                            else if (feldXml.Attributes("Typ").First().Value == "Ganzzahl")
                                feld = new FeldGanzzahl(feldXml, Random);
                            else if (feldXml.Attributes("Typ").First().Value == "GebrZahl")
                                feld = new FeldGebrZahl(feldXml, Random);
                            else if (feldXml.Attributes("Typ").First().Value == "Freitext")
                                feld = new FeldFreitext(feldXml, Random);
                        }

                        string wert = feld.Generieren(out bool schlecht);
                        Feldliste.Add(feld.Nummer, wert);
                        zeile += wert + Konfiguration.Feldtrennzeichen;

                        if (schlecht)
                            Logger.LoggenSchlechtfeld(new Schlechtfeld { Dateiname = dateiname, Zeile = zeilennummer + 1, Feld = feld.Nummer });
                    }
                }
            }

            zeile = zeile.Substring(0, zeile.Length - 1) + Konfiguration.Zeilentrennzeichen;

            return zeile;
        }
    }
}
