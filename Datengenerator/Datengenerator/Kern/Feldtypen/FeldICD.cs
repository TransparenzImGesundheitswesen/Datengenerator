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

        public override string Generieren(out bool schlecht)
        {
            if (SchlechtdatenGenerieren && Random.Next(0, SchlechtdatenWahrscheinlichkeit) == 0)
            {
                schlecht = true;

                if (Random.Next(0, 2) == 0)
                    return "AB"; // zu kurz
                else
                    return "F5E3155D5FF48"; // zu lang
            }
            else
            {
                schlecht = false;
                return Schlüsselverzeichnismanager.ZufälligerEintrag(SchlüsselverzeichnisName);
            }
        }
    }
}
