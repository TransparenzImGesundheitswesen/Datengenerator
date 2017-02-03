using System;
using System.Collections.Generic;

namespace Datengenerator.Konfig
{
    public static class Konfiguration
    {
        public static readonly List<string> Xsd = new List<string>();
        public static readonly string Xml;
        public static readonly bool Validieren;

        static Konfiguration()
        {
            List<string> einträge = new List<string>(System.IO.File.ReadAllLines("Datengenerator.konfig"));

            foreach (string eintrag in einträge)
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
                }
            }
        }
    }
}
