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

        public override string Generieren()
        {
            if (SchlechtdatenGenerieren && Random.Next(0, SchlechtdatenWahrscheinlichkeit) == 0)
                return "FG";
            else
                return Schlüsselverzeichnismanager.ZufälligerEintrag(SchlüsselverzeichnisName);
        }
    }
}
