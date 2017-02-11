using System;
using System.Xml.Linq;

namespace Datengenerator.Kern
{
    class FeldICD : Feld
    {
        public string SchlüsselverzeichnisName;

        public FeldICD(XElement xml, Random r) : base(xml, r)
        {
            SchlüsselverzeichnisName = "ICD";
        }

        public override string Generieren()
        {
            return Schlüsselverzeichnismanager.ZufälligerEintrag(SchlüsselverzeichnisName);
        }
    }
}
