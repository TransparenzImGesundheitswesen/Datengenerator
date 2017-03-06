using System;
using System.Xml.Linq;

namespace Datengenerator.Kern
{
    class FeldGebrZahl : Feld
    {
        public int StellenVor;
        public int StellenNach;

        public FeldGebrZahl(XElement xml, Random r) : base(xml, r)
        {
            string[] komponenten = xml.Element("Stellen").Value.Split(',');

            StellenVor = int.Parse(komponenten[0]);
            StellenNach = int.Parse(komponenten[1]);
        }

        public override string Generieren()
        {
            double max = Math.Pow(10, StellenVor);
            return string.Format("{0:F" + StellenNach + "}", Random.NextDouble() * max);
        }
    }
}
