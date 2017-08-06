using System;
using System.Xml.Linq;

namespace Datengenerator.Kern
{
    class FeldGOP : Feld
    {
        public string SchlüsselverzeichnisName;

        public FeldGOP(XElement xml, Random r) : base(xml, r)
        {
            SchlüsselverzeichnisName = "GOP";
        }

        public override string Generieren(out bool schlecht)
        {
            if (SchlechtdatenGenerieren && Random.Next(0, SchlechtdatenWahrscheinlichkeit) == 0)
            {
                schlecht = true;
                return "GOPGOPGOP";
            }
            else
            {
                schlecht = false;
                return Schlüsselverzeichnismanager.ZufälligerEintrag(SchlüsselverzeichnisName);
            }
        }
    }
}
