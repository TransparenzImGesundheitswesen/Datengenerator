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

        public FeldQuartalDatum(XElement xml, Random r, Dictionary<string, string> dateiattribute, Dictionary<string, string> feldliste) : base(xml, r)
        {
            Dateiattribute = dateiattribute;
            Feldliste = feldliste;

            if (xml.Elements("Format").Any())
                Format = xml.Element("Format").Value;

            if (xml.Elements("GrößerGleich").Any())
                GrößerGleich = xml.Element("GrößerGleich").Value;
        }

        public override string Generieren()
        {
            switch (Format)
            {
                case "JJJJQ":
                    return GenerierenQuartal();
                case "JJJJMMTT":
                    return GenerierenDatum();
                default:
                    throw new NotImplementedException();
            }
        }

        private string GenerierenQuartal()
        {
            if (SchlechtdatenGenerieren && Random.Next(0, SchlechtdatenWahrscheinlichkeit) == 0)
                return "QQQQQ";
            else
            {
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
                    } while (GrößerGleich != null && string.Compare(Feldliste[GrößerGleich], rückgabe) > 0);

                    return rückgabe;
                }
            }
        }

        private string GenerierenDatum()
        {
            if (SchlechtdatenGenerieren && Random.Next(0, SchlechtdatenWahrscheinlichkeit) == 0)
                return "JJJJMMTT";
            else
                return string.Format("2017{0:00}01", Random.Next(1, 13));
        }
    }
}
