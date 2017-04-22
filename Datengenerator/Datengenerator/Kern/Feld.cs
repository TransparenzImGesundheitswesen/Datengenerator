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

        private List<string> blindtexte = new List<string> { "Narf", "Troz", "Zort", "Fjort", "Poit" };


        public Feld(XElement xml, Random r, int schlechtdatenWahrscheinlichkeit)
        {
            Nummer = xml.Attribute("Nummer").Value;
            Name = xml.Element("Name").Value;
            Art = (Feldart)Enum.Parse(typeof(Feldart), xml.Element("Art").Value);

            Random = r;
            SchlechtdatenGenerieren = schlechtdatenWahrscheinlichkeit != 0;
            SchlechtdatenWahrscheinlichkeit = schlechtdatenWahrscheinlichkeit;

            if (xml.Elements("Format").Any())
                Format = xml.Element("Format").Value;
            else
                Format = "";
        }

        public Feld(XElement xml, Random r) : this(xml, r, 0)
        {
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
                    return blindtexte[Random.Next(0, blindtexte.Count)];
            }
        }

        private string GenerierenQuartal()
        {
            if (SchlechtdatenGenerieren && Random.Next(0, SchlechtdatenWahrscheinlichkeit) == 0)
                return "QQQQQ";
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
