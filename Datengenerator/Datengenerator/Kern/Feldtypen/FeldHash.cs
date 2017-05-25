using System;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;

namespace Datengenerator.Kern
{
    class FeldHash : Feld
    {
        public FeldHash(XElement xml, Random r, int schlechtdatenWahrscheinlichkeit) : base(xml, r, schlechtdatenWahrscheinlichkeit)
        {
        }

        public override string Generieren()
        {
            if (SchlechtdatenGenerieren && Random.Next(0, SchlechtdatenWahrscheinlichkeit) == 0)
            {
                if (Random.Next(0, 2) == 0)
                    return "ABCDEF";
                else
                    return "F5E3155D5FF487546DF7E1ABFZF1BE479942E665"; // enthält ein Z
            }
            else
            {
                RIPEMD160 ripe = RIPEMD160.Create();
                byte[] bytes = Encoding.ASCII.GetBytes(Guid.NewGuid().ToString());
                byte[] hash = ripe.ComputeHash(bytes);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hash.Length; i++)
                    sb.Append(hash[i].ToString("X2"));

                return sb.ToString();
            }
        }
    }
}
