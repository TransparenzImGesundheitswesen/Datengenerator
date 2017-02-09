using System;
using System.Xml.Linq;

namespace Datengenerator.Kern
{
    class FeldKalTag : Feld
    {
        public FeldKalTag(XElement xml, Random r) : base(xml, r)
        {
        }

        public override string Generieren()
        {
            return Random.Next(1, 32).ToString();
        }
    }
}
