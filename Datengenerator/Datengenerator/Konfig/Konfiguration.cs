using System;
using System.Collections.Generic;
using System.Linq;

namespace Datengenerator.Konfig
{
    public static class Konfiguration
    {
        public static readonly List<string> Xsd = new List<string>();
        public static readonly string Xml;
        public static readonly bool Validieren;
        public static readonly int SchlechtdatenWahrscheinlichkeit;
        public static readonly int SchlechtdatenWahrscheinlichkeitFremdschlüssel;
        public static readonly int AnzahlZeilen;

        public static string Zeichensatz = "";
        public static string Feldtrennzeichen = "";
        public static string Zeilentrennzeichen = "";

        static Konfiguration()
        {
            List<string> einträge = new List<string>(System.IO.File.ReadAllLines("Datengenerator.konfig"));

            foreach (string eintrag in einträge.Where(m => m.Length > 0 && m.Substring(0, 1) != "#"))
            {
                string[] komponenten = eintrag.Split(':');

                switch (komponenten[0])
                {
                    case "XSD":
                        Xsd.Add(komponenten[1].Trim());
                        break;
                    case "XML":
                        Xml = komponenten[1].Trim();
                        break;
                    case "Validieren":
                        Validieren = Convert.ToBoolean(int.Parse(komponenten[1].Trim()));
                        break;
                    case "SchlechtdatenWahrscheinlichkeit":
                        SchlechtdatenWahrscheinlichkeit = int.Parse(komponenten[1].Trim());
                        break;
                    case "SchlechtdatenWahrscheinlichkeitFremdschlüssel":
                        SchlechtdatenWahrscheinlichkeitFremdschlüssel = int.Parse(komponenten[1].Trim());
                        break;
                    case "AnzahlZeilen":
                        AnzahlZeilen = int.Parse(komponenten[1].Trim());
                        break;
                }
            }
        }
    }
}
