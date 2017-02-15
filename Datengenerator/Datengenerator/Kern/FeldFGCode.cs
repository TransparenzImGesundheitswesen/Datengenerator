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
            return Schlüsselverzeichnismanager.ZufälligerEintrag(SchlüsselverzeichnisName);
        }
    }
}
