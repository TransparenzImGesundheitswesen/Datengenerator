using System;
using System.Xml.Linq;

namespace Datengenerator.Kern
{
    class FeldPLZ : Feld
    {
        public string SchlüsselverzeichnisName;

        public FeldPLZ(XElement xml, Random r) : base(xml, r)
        {
            SchlüsselverzeichnisName = "PLZ";
        }

        public override string Generieren(out bool schlecht)
        {
            if (SchlechtdatenGenerieren && Random.Next(0, SchlechtdatenWahrscheinlichkeit) == 0)
            {
                schlecht = true;
                return "ABCDE";
            }
            else
            {
                schlecht = false;
                return Schlüsselverzeichnismanager.ZufälligerEintrag(SchlüsselverzeichnisName);
            }
        }
    }
}
