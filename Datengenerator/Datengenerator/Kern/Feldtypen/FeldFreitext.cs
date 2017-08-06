using System;
using System.Xml.Linq;


namespace Datengenerator.Kern
{
    class FeldFreitext : Feld
    {
        private int stellen;
        private const string blindtext = "Zwei flinke Boxer jagen die quirlige Eva und ihren Mops durch Sylt. Zwölf Boxkämpfer jagen Viktor quer über den großen Sylter Deich. Vogel Quax zwickt Johnys Pferd Bim. Sylvia wagt quick den Jux bei Pforzheim. Polyfon zwitschernd aßen Mäxchens Vögel Rüben, Joghurt und Quark. Fix, Schwyz!, quäkt Jürgen blöd vom Paß. Victor jagt zwölf Boxkämpfer quer über den großen Sylter Deich. Falsches Üben von Xylophonmusik quält jeden größeren Zwerg. Heizölrückstoßabdämpfung. Franz jagt im komplett verwahrlosten Taxi quer durch Bayern.";

        public FeldFreitext(XElement xml, Random r) : base(xml, r)
        {
            stellen = int.Parse(xml.Element("Stellen").Value.Replace("<=", ""));
        }

        public override string Generieren(out bool schlecht)
        {
            if (SchlechtdatenGenerieren && Random.Next(0, SchlechtdatenWahrscheinlichkeit) == 0)
            {
                schlecht = true;
                return blindtext.Substring(0, stellen + 1);
            }
            else
            {
                schlecht = false;
                return blindtext.Substring(0, stellen);
            }
        }
    }
}
