using System;
using System.Xml.Linq;

namespace Datengenerator.Kern
{
    class FeldGanzzahl : Feld
    {
        private int stellen;

        public FeldGanzzahl(XElement xml, Random r, int schlechtdatenWahrscheinlichkeit) : base(xml, r, schlechtdatenWahrscheinlichkeit)
        {
            stellen = int.Parse(xml.Element("Stellen").Value.Replace("<=", ""));
        }

        public override string Generieren()
        {
            if (SchlechtdatenGenerieren && Random.Next(0, SchlechtdatenWahrscheinlichkeit) == 0)
            {
                if (Random.Next(0, 2) == 0)
                    return "ABC";
                else
                    return "3,14";
            }
            else
            {
                int max = (int)Math.Pow(10, stellen);
                return Random.Next(1, max).ToString();
            }
        }
    }
}
