using System;
using System.Xml.Linq;

namespace Datengenerator.Kern
{
    class FeldKonstant : Feld
    {
        public string Konstant;

        public FeldKonstant(XElement xml, Random r) : base(xml, r)
        {
            Konstant = xml.Element("Konstant").Value;
        }

        public override string Generieren(out bool schlecht)
        {
            if (SchlechtdatenGenerieren && Random.Next(0, SchlechtdatenWahrscheinlichkeit) == 0)
            {
                schlecht = true;
                return "Narf!";
            }
            else
            {
                schlecht = false;
                return Konstant;
            }
        }
    }
}
