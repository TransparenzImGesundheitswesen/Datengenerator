using System;
using System.Xml.Linq;

namespace Datengenerator.Kern
{
    class FeldGanzzahl : Feld
    {
        public int Stellen;

        public FeldGanzzahl(XElement xml, Random r) : base(xml, r)
        {
            Stellen = int.Parse(xml.Element("Stellen").Value.Replace("<=", ""));
        }

        public override string Generieren()
        {
            int max = (int)Math.Pow(10, Stellen);
            return Random.Next(1, max).ToString();
        }
    }
}
