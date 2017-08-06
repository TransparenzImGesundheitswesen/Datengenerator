using System;
using System.Xml.Linq;

namespace Datengenerator.Kern
{
    class FeldKalTag : Feld
    {
        public FeldKalTag(XElement xml, Random r) : base(xml, r)
        {
        }

        public override string Generieren(out bool schlecht)
        {
            if (SchlechtdatenGenerieren && Random.Next(0, SchlechtdatenWahrscheinlichkeit) == 0)
            {
                schlecht = true;

                if (Random.Next(0, 2) == 0)
                    return "0";
                else
                    return "32";
            }
            else
            {
                schlecht = false;
                return Random.Next(1, 32).ToString();
            }
        }
    }
}
