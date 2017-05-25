using System;
using System.Xml.Linq;

namespace Datengenerator.Kern
{
    class FeldGebrZahl : Feld
    {
        public int StellenVor;
        public int StellenNach;

        public FeldGebrZahl(XElement xml, Random r, int schlechtdatenWahrscheinlichkeit) : base(xml, r, schlechtdatenWahrscheinlichkeit)
        {
            string[] komponenten = xml.Element("Stellen").Value.Split(',');

            StellenNach = int.Parse(komponenten[1]);
            StellenVor = int.Parse(komponenten[0]) - StellenNach;
        }

        public override string Generieren()
        {
            if (SchlechtdatenGenerieren && Random.Next(0, SchlechtdatenWahrscheinlichkeit) == 0)
                return "ABC";
            else
            {
                double max = Math.Pow(10, StellenVor);
                return string.Format("{0:F" + StellenNach + "}", Random.NextDouble() * max);
            }
        }
    }
}
