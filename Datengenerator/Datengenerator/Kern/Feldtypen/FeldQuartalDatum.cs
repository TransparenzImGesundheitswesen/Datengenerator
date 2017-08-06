using Datengenerator.Konfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Datengenerator.Kern
{
    class FeldQuartalDatum : Feld
    {
        public string Format;
        public Dictionary<string, string> Dateiattribute;
        public Dictionary<string, string> Feldliste;
        public string GrößerGleich;
        public string LiegtIn;

        public FeldQuartalDatum(XElement xml, Random r, Dictionary<string, string> dateiattribute, Dictionary<string, string> feldliste) : base(xml, r)
        {
            Dateiattribute = dateiattribute;
            Feldliste = feldliste;

            if (xml.Elements("Format").Any())
                Format = xml.Element("Format").Value;

            if (xml.Elements("GrößerGleich").Any())
                GrößerGleich = xml.Element("GrößerGleich").Value;

            if (xml.Elements("LiegtIn").Any())
                LiegtIn = xml.Element("LiegtIn").Value;
        }

        public override string Generieren(out bool schlecht)
        {
            switch (Format)
            {
                case "JJJJQ":
                    return GenerierenQuartal(out schlecht);
                case "JJJJMMTT":
                    return GenerierenDatum(out schlecht);
                default:
                    throw new NotImplementedException();
            }
        }

        private string GenerierenQuartal(out bool schlecht)
        {
            if (SchlechtdatenGenerieren && Random.Next(0, SchlechtdatenWahrscheinlichkeit) == 0)
            {
                schlecht = true;
                return "QQQQQ";
            }
            else
            {
                schlecht = false;

                if (Name.Contains("Geburts"))
                    return string.Format("{0}{1}", 1950 + Random.Next(0, 60), Random.Next(1, 5));
                else
                {
                    string rückgabe;

                    do
                    {
                        if (Konfiguration.Quartalsliste.Count > 0)
                            rückgabe = Konfiguration.Quartalsliste[Random.Next(0, Konfiguration.Quartalsliste.Count)];
                        else if (Dateiattribute.Keys.Contains("Jahr"))
                            rückgabe = string.Format("{0}{1}", Dateiattribute["Jahr"], Random.Next(1, 5));
                        else
                            rückgabe = string.Format("2017{0}", Random.Next(1, 5));
                    } while (GrößerGleich != null && string.Compare(Feldliste[GrößerGleich], rückgabe) > 0 && Feldliste[GrößerGleich] != "QQQQQ");

                    return rückgabe;
                }
            }
        }

        private string GenerierenDatum(out bool schlecht)
        {
            if ((SchlechtdatenGenerieren && Random.Next(0, SchlechtdatenWahrscheinlichkeit) == 0)
                || (LiegtIn != null && Feldliste[LiegtIn].Length != 5))
            {
                schlecht = true;
                return "JJJJMMTT";
            }
            else
            {
                schlecht = false;

                if (LiegtIn != null)
                {
                    string jahr = Feldliste[LiegtIn].Substring(0, 4);
                    string quartal = Feldliste[LiegtIn].Substring(4, 1);
                    string monat = "";

                    switch (quartal)
                    {
                        case "1":
                            monat = string.Format("{0:00}", Random.Next(1, 4));
                            break;
                        case "2":
                            monat = string.Format("{0:00}", Random.Next(4, 7));
                            break;
                        case "3":
                            monat = string.Format("{0:00}", Random.Next(7, 10));
                            break;
                        case "4":
                            monat = string.Format("{0:00}", Random.Next(10, 13));
                            break;
                    }

                    return string.Format("{0}{1}{2:00}", jahr, monat, Random.Next(1, 28));
                }
                else
                    return string.Format("2017{0:00}{1:00}", Random.Next(1, 13), Random.Next(1, 28));
            }
        }
    }
}
