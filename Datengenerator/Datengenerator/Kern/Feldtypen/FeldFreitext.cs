using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;


namespace Datengenerator.Kern
{
    class FeldFreitext : Feld
    {
        private int stellen;
        private bool variableLänge;
        private const string blindtext = "Zwei flinke Boxer jagen die quirlige Eva und ihren Mops durch Sylt. Zwölf Boxkämpfer jagen Viktor quer über den großen Sylter Deich. Vogel Quax zwickt Johnys Pferd Bim. Sylvia wagt quick den Jux bei Pforzheim. Polyfon zwitschernd aßen Mäxchens Vögel Rüben, Joghurt und Quark. Fix, Schwyz!, quäkt Jürgen blöd vom Paß. Victor jagt zwölf Boxkämpfer quer über den großen Sylter Deich. Falsches Üben von Xylophonmusik quält jeden größeren Zwerg. Heizölrückstoßabdämpfung. Franz jagt im komplett verwahrlosten Taxi quer durch Bayern.";

        public string Dateiattribut;
        public Dictionary<string, string> Dateiattribute;

        public FeldFreitext(XElement xml, Random r, Dictionary<string, string> dateiattribute) : base(xml, r)
        {
            variableLänge = xml.Element("Stellen").Value.Contains("<=");
            stellen = int.Parse(xml.Element("Stellen").Value.Replace("<=", ""));

            Dateiattribute = dateiattribute;

            if (xml.Elements("Dateiattribut").Any())
                Dateiattribut = xml.Element("Dateiattribut").Value;
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

                if (Dateiattribut != null)
                    return Dateiattribute[Dateiattribut];
                else if (variableLänge)
                    return blindtext.Substring(0, Random.Next(1, stellen));
                else
                    return blindtext.Substring(0, stellen);
            }
        }
    }
}
