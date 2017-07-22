using Datengenerator.Konfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Datengenerator.Kern
{
    class Feld
    {
        public string Nummer;
        public string Name;
        public Feldart Art;
        public Feldeigenschaft Eigenschaft;
        public string Format;
        public string Erläuterung;

        public readonly Random Random;
        public readonly bool SchlechtdatenGenerieren;
        public readonly int SchlechtdatenWahrscheinlichkeit;


        public Feld(XElement xml, Random r)
        {
            Nummer = xml.Attribute("Nummer").Value;
            Name = xml.Element("Name").Value;
            Art = (Feldart)Enum.Parse(typeof(Feldart), xml.Element("Art").Value);

            Random = r;
            SchlechtdatenGenerieren = Konfiguration.SchlechtdatenWahrscheinlichkeit != 0;
            SchlechtdatenWahrscheinlichkeit = Konfiguration.SchlechtdatenWahrscheinlichkeit;

            if (xml.Elements("Format").Any())
                Format = xml.Element("Format").Value;
            else
                Format = "";
        }

        public virtual string Generieren()
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
                if (Name.Contains("Geburts"))
                    return string.Format("{0}{1}", 1950 + Random.Next(0, 60), Random.Next(1, 5));
                else
                    return string.Format("2017{0}", Random.Next(1, 5));
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
