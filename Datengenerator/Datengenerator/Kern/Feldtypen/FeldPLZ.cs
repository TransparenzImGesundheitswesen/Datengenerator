using System;
using System.Xml.Linq;

namespace Datengenerator.Kern
{
    class FeldPLZ : Feld
    {
        public FeldPLZ(XElement xml, Random r) : base(xml, r)
        {
        }

        public override string Generieren(out bool schlecht)
        {
            if (SchlechtdatenGenerieren && Random.Next(0, SchlechtdatenWahrscheinlichkeit) == 0)
            {
                schlecht = true;
                return "ABCDE";
            }
            else
            {
                schlecht = false;
                return string.Format("{0:00000}", Random.Next(1000, 100000));
            }
        }
    }
}
