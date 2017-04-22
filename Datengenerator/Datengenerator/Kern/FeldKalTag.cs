using System;
using System.Xml.Linq;

namespace Datengenerator.Kern
{
    class FeldKalTag : Feld
    {
        public FeldKalTag(XElement xml, Random r, int schlechtdatenWahrscheinlichkeit) : base(xml, r, schlechtdatenWahrscheinlichkeit)
        {
        }

        public override string Generieren()
        {
            if (SchlechtdatenGenerieren && Random.Next(0, SchlechtdatenWahrscheinlichkeit) == 0)
            {
                if (Random.Next(0, 2) == 0)
                    return "0";
                else
                    return "32";
            }
            else
                return Random.Next(1, 32).ToString();
        }
    }
}
