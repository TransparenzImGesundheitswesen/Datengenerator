using System;
using System.Xml.Linq;

namespace Datengenerator.Kern
{
    class FeldGOP : Feld
    {
        public string SchlüsselverzeichnisName;

        public FeldGOP(XElement xml, Random r, int schlechtdatenWahrscheinlichkeit) : base(xml, r, schlechtdatenWahrscheinlichkeit)
        {
            SchlüsselverzeichnisName = "GOP";
        }

        public override string Generieren()
        {
            if (SchlechtdatenGenerieren && Random.Next(0, SchlechtdatenWahrscheinlichkeit) == 0)
                return "GOPGOPGOP";
            else
                return Schlüsselverzeichnismanager.ZufälligerEintrag(SchlüsselverzeichnisName);
        }
    }
}
