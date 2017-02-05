using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Datengenerator.Kern
{
    class FeldIK : Feld
    {
        public string SchlüsselverzeichnisName;

        public FeldIK(XElement xml, Random r) : base(xml, r)
        {
            SchlüsselverzeichnisName = xml.Element("Schlüsselverzeichnis").Value;
        }

        public override string Generieren()
        {
            return Schlüsselverzeichnismanager.ZufälligerEintrag(SchlüsselverzeichnisName);
        }
    }
}
