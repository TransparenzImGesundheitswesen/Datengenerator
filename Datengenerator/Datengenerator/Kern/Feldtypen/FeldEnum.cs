using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Datengenerator.Kern
{
    class FeldEnum : Feld
    {
        public List<string> ZulässigeWerte;

        public FeldEnum(XElement xml, Random r) : base(xml, r)
        {
            ZulässigeWerte = xml.Descendants("Wert").Select(m => m.Value).ToList();
        }

        public override string Generieren()
        {
            if (SchlechtdatenGenerieren && Random.Next(0, SchlechtdatenWahrscheinlichkeit) == 0)
                return "999";
            else
                return ZulässigeWerte[Random.Next(0, ZulässigeWerte.Count)];
        }
    }
}
