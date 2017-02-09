using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Datengenerator.Kern
{
    class FeldVsTage : Feld
    {
        public FeldVsTage(XElement xml, Random r) : base(xml, r)
        {
        }

        public override string Generieren()
        {
            return (1 + Random.Next(91)).ToString();
        }
    }
}
