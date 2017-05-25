using System;
using System.Xml.Linq;

namespace Datengenerator.Kern
{
    class FeldGebrZahl : Feld
    {
        private int stellenVor;
        private int stellenNach;

        public FeldGebrZahl(XElement xml, Random r, int schlechtdatenWahrscheinlichkeit) : base(xml, r, schlechtdatenWahrscheinlichkeit)
        {
            string[] komponenten = xml.Element("Stellen").Value.Split(',');

            stellenNach = int.Parse(komponenten[1]);
            stellenVor = int.Parse(komponenten[0]) - stellenNach;
        }

        public override string Generieren()
        {
            if (SchlechtdatenGenerieren && Random.Next(0, SchlechtdatenWahrscheinlichkeit) == 0)
                return "ABC";
            else
            {
                double max = Math.Pow(10, stellenVor);
                return string.Format("{0:F" + stellenNach + "}", Random.NextDouble() * max);
            }
        }
    }
}
