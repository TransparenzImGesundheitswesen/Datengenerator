using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Datengenerator.Kern
{
    class FeldPLZ : Feld
    {
        public FeldPLZ(XElement xml, Random r) : base(xml, r)
        {
        }

        public override string Generieren()
        {
            return string.Format("{0:00000}", Random.Next(1000, 100000));
        }
    }
}
