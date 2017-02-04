﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Datengenerator.Kern
{
    class Feld
    {
        public string Nummer;
        public string Name;
        public Feldart Art;
        public int Stellen;
        public Feldeigenschaft Eigenschaft;
        public string Format;
        public string Erläuterung;
        public readonly Random Random;

        public Feld(XElement xml, Random r)
        {
            Nummer = xml.Attribute("Nummer").Value;
            Name = xml.Element("Name").Value;
            Art = (Feldart)Enum.Parse(typeof(Feldart), xml.Element("Art").Value);

            Random = r;

            if (xml.Elements("Format").Any())
                Format = xml.Element("Format").Value;
            else
                Format = "";
        }

        public virtual string Generieren()
        {
            if (Art == Feldart.K && Random.Next(0, 2) == 0)
                return "";
            else
            {
                switch (Format)
                {
                    case "JJJJQ":
                        return string.Format("2017{0}", Random.Next(1, 5));
                    default:
                        return "Narf";
                }
            }
        }
    }
}
