using System;
using System.Xml.Linq;

namespace Datengenerator.Kern
{
    class FeldICD : Feld
    {
        public string SchlüsselverzeichnisName;

        public FeldICD(XElement xml, Random r) : base(xml, r, null)
        {
            SchlüsselverzeichnisName = "ICD";
        }

        public override string Generieren()
        {
            if (SchlechtdatenGenerieren && Random.Next(0, SchlechtdatenWahrscheinlichkeit) == 0)
            {
                if (Random.Next(0, 2) == 0)
                    return "AB"; // zu kurz
                else
                    return "F5E3155D5FF48"; // zu lang
            }
            else
                return Schlüsselverzeichnismanager.ZufälligerEintrag(SchlüsselverzeichnisName);
        }
    }
}
