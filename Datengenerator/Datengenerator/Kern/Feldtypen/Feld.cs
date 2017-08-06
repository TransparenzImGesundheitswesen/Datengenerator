using Datengenerator.Konfig;
using System;
using System.Xml.Linq;

namespace Datengenerator.Kern
{
    class Feld
    {
        public string Nummer;
        public string Name;
        public Feldart Art;

        public readonly Random Random;
        public readonly bool SchlechtdatenGenerieren;
        public readonly int SchlechtdatenWahrscheinlichkeit;


        public Feld(XElement xml, Random r)
        {
            Nummer = xml.Attribute("Nummer").Value;
            Name = xml.Element("Name").Value;
            Art = (Feldart)Enum.Parse(typeof(Feldart), xml.Element("Art").Value);

            Random = r;
            SchlechtdatenGenerieren = Konfiguration.SchlechtdatenWahrscheinlichkeit != 0;
            SchlechtdatenWahrscheinlichkeit = Konfiguration.SchlechtdatenWahrscheinlichkeit;
        }

        public virtual string Generieren(out bool schlecht)
        {
            throw new NotImplementedException();
        }
    }
}
