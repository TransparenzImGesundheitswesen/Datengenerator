using System;
using System.Xml.Linq;

namespace Datengenerator.Kern
{
    class FeldGebrZahl : Feld
    {
        private int stellenVor;
        private int stellenNach;

        public FeldGebrZahl(XElement xml, Random r) : base(xml, r)
        {
            string[] komponenten = xml.Element("Stellen").Value.Split(',');

            stellenNach = int.Parse(komponenten[1]);
            stellenVor = int.Parse(komponenten[0]) - stellenNach;
        }

        public override string Generieren(out bool schlecht)
        {
            if (SchlechtdatenGenerieren && Random.Next(0, SchlechtdatenWahrscheinlichkeit) == 0)
            {
                schlecht = true;
                return "ABC";
            }
            else
            {
                schlecht = false;

                double max = Math.Pow(10, stellenVor);
                return string.Format("{0:F" + stellenNach + "}", Random.NextDouble() * max);
            }
        }
    }
}
