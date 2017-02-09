using System;
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

        public override string Generieren()
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
