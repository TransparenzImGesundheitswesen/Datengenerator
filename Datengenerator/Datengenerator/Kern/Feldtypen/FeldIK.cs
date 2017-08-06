using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Datengenerator.Kern
{
    class FeldIK : Feld
    {
        public string Dateiattribut;
        public Dictionary<string, string> Dateiattribute;

        public FeldIK(XElement xml, Random r, Dictionary<string, string> dateiattribute) : base(xml, r)
        {
            Dateiattribute = dateiattribute;
            Dateiattribut = xml.Element("Dateiattribut").Value;
        }

        public override string Generieren(out bool schlecht)
        {
            if (SchlechtdatenGenerieren && Random.Next(0, SchlechtdatenWahrscheinlichkeit) == 0)
            {
                schlecht = true;

                if (Random.Next(0, 2) == 0)
                    return "ABC";
                else
                    return "012345678";
            }
            else
            {
                schlecht = false;
                return Dateiattribute[Dateiattribut];
            }
        }
    }
}
