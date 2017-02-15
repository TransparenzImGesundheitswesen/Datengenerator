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

        public override string Generieren()
        {
            return Schlüsselverzeichnismanager.ZufälligerEintrag(SchlüsselverzeichnisName);
        }
    }
}
