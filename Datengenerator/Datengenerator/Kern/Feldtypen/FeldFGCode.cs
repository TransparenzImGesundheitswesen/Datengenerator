using System;
using System.Xml.Linq;

namespace Datengenerator.Kern
{
    class FeldFGCode : Feld
    {
        public string SchlüsselverzeichnisName;

        public FeldFGCode(XElement xml, Random r) : base(xml, r)
        {
            SchlüsselverzeichnisName = "FGCode";
        }

        public override string Generieren(out bool schlecht)
        {
            if (SchlechtdatenGenerieren && Random.Next(0, SchlechtdatenWahrscheinlichkeit) == 0)
            {
                schlecht = true;
                return "FG";
            }
            else
            {
                schlecht = false;
                return Schlüsselverzeichnismanager.ZufälligerEintrag(SchlüsselverzeichnisName);
            }
        }
    }
}
