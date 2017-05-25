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
        public readonly Random Prop;

        public FeldVsTage(XElement xml, Random r, Random prop, int schlechtdatenWahrscheinlichkeit) : base(xml, r, schlechtdatenWahrscheinlichkeit)
        {
            Prop = prop;
        }

        public override string Generieren()
        {
            if (SchlechtdatenGenerieren && Random.Next(0, SchlechtdatenWahrscheinlichkeit) == 0)
            {
                switch (Random.Next(0, 3))
                {
                    case 0:
                        return "0";
                    case 1:
                        return "94";
                    default:
                        return "A";
                }
            }
            else
                return (1 + Prop.Next(91)).ToString();
        }
    }
}
