using System;
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
            if (SchlechtdatenGenerieren && Random.Next(0, SchlechtdatenWahrscheinlichkeit) == 0)
            {
                if (Random.Next(0, 2) == 0)
                    return "ABC";
                else
                    return "012345678";
            }
            else
                return Schlüsselverzeichnismanager.ZufälligerEintrag(SchlüsselverzeichnisName);
        }
    }
}
