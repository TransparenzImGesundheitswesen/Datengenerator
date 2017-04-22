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

        private List<string> blindtexte = new List<string> { "Narf", "Troz", "Zort", "Fjort", "Poit" };


        public Feld(XElement xml, Random r, bool schlechtdatenGenerieren)
        {
            Nummer = xml.Attribute("Nummer").Value;
            Name = xml.Element("Name").Value;
            Art = (Feldart)Enum.Parse(typeof(Feldart), xml.Element("Art").Value);

            Random = r;
            SchlechtdatenGenerieren = schlechtdatenGenerieren;

            if (xml.Elements("Format").Any())
                Format = xml.Element("Format").Value;
            else
                Format = "";
        }

        public Feld(XElement xml, Random r) : this(xml, r, false)
        {
        }

        public virtual string Generieren()
        {
            switch (Format)
            {
                case "JJJJQ":
                    return string.Format("2017{0}", Random.Next(1, 5));
                case "JJJJMMTT":
                    return string.Format("2017{0:00}01", Random.Next(1, 13));
                default:
                    return blindtexte[Random.Next(0, blindtexte.Count)];
            }
        }
    }
}
