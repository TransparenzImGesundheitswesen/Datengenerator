using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;

namespace Datengenerator.Kern
{
    class FeldHash : Feld
    {
        public FeldHash(XElement xml, Random r) : base(xml, r)
        {
        }

        public override string Generieren(out bool schlecht)
        {
            if (SchlechtdatenGenerieren && Random.Next(0, SchlechtdatenWahrscheinlichkeit) == 0)
            {
                schlecht = true;

                if (Random.Next(0, 2) == 0)
                    return "ABCDEF";
                else
                    return "F5E3155D5FF487546DF7E1ABFZF1BE479942E665"; // enthält ein Z
            }
            else
            {
                schlecht = false;

                return string.Concat(Enumerable.Range(0, 40).Select(_ => Random.Next(16).ToString("X")));
            }
        }
    }
}
