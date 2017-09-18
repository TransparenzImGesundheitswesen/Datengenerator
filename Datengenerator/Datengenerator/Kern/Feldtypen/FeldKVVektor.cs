using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Datengenerator.Kern
{
    class FeldKVVektor : Feld
    {
        public List<string> KVen = new List<string> {
            "10000000000000000",
            "01000000000000000",
            "00100000000000000",
            "00010000000000000",
            "00001000000000000",
            "00000100000000000",
            "00000010000000000",
            "00000001000000000",
            "00000000100000000",
            "00000000010000000",
            "00000000001000000",
            "00000000000100000",
            "00000000000010000",
            "00000000000001000",
            "00000000000000100",
            "00000000000000010",
            "00000000000000001"
        };

        public FeldKVVektor(XElement xml, Random r) : base(xml, r)
        {
        }

        public override string Generieren(out bool schlecht)
        {
            if (SchlechtdatenGenerieren && Random.Next(0, SchlechtdatenWahrscheinlichkeit) == 0)
            {
                schlecht = true;

                if (Random.Next(0, 2) == 0)
                    return "04";
                else
                    return "AB";
            }
            else
            {
                schlecht = false;
                return KVen[Random.Next(KVen.Count)];
            }
        }
    }
}
