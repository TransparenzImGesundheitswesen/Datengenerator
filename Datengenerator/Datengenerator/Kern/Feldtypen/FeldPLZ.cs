using System;
using System.Xml.Linq;

namespace Datengenerator.Kern
{
    class FeldPLZ : Feld
    {
        public FeldPLZ(XElement xml, Random r) : base(xml, r, null)
        {
        }

        public override string Generieren()
        {
            if (SchlechtdatenGenerieren && Random.Next(0, SchlechtdatenWahrscheinlichkeit) == 0)
                return "ABCDE";
            else
                return string.Format("{0:00000}", Random.Next(1000, 100000));
        }
    }
}
