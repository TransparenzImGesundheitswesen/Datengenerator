using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Datengenerator.Kern
{
    class FeldIK : Feld
    {
        public string Dateiattribut;

        public FeldIK(XElement xml, Random r, Dictionary<string, string> dateiattribute) : base(xml, r, dateiattribute)
        {
            Dateiattribute = dateiattribute;
            Dateiattribut = xml.Element("Dateiattribut").Value;
        }

        public override string Generieren()
        {
            if (SchlechtdatenGenerieren && Random.Next(0, SchlechtdatenWahrscheinlichkeit) == 0)
            {
                if (Random.Next(0, 2) == 0)
                    return "ABC";
                else
                    return "012345678";
            }
            else
                return Dateiattribute[Dateiattribut];
        }
    }
}
