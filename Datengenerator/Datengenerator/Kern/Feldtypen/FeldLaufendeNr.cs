using System;
using System.Xml.Linq;

namespace Datengenerator.Kern
{
    class FeldLaufendeNr : Feld
    {
        private int stellen;

        public FeldLaufendeNr(XElement xml, Random r) : base(xml, r)
        {
            stellen = int.Parse(xml.Element("Stellen").Value.Replace("<=", ""));
        }

        public override string Generieren(out bool schlecht)
        {
            if (SchlechtdatenGenerieren && Random.Next(0, SchlechtdatenWahrscheinlichkeit) == 0)
            {
                schlecht = true;

                if (Random.Next(0, 2) == 0)
                    return "ABC";
                else
                    return "3,14";
            }
            else
            {
                schlecht = false;

                int max = (int)Math.Pow(10, stellen);
                return Random.Next(1, max).ToString();
            }
        }
    }
}
