using Datengenerator.Kern;
using System;
using System.Collections.Generic;
using System.IO;
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
        private static string dateiname = "";
        public static string Pfad = "";

        public static List<string> Quartalsliste = new List<string>();

        public static Dictionary<string, List<string>> Dateiattribute = new Dictionary<string, List<string>>();
        public static List<Dictionary<string, string>> DateiattributeKombinationen = new List<Dictionary<string, string>>();


        static Konfiguration()
        {
            List<string> einträge = new List<string>(System.IO.File.ReadAllLines("Datengenerator.konfig"));

            foreach (string eintrag in einträge.Where(m => m.Length > 0 && m.Substring(0, 1) != "#"))
            {
                string[] komponenten = eintrag.Split(':');

                if (komponenten[0].StartsWith("_"))
                {
                    string attribut = komponenten[0].Replace("_", "");

                    if (komponenten[1].Length > 0)
                    {
                        List<string> werte = komponenten[1].Split(';').ToList();
                        Dateiattribute.Add(attribut, werte);
                    }
                }


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
                    case "Quartalsliste":
                        if (komponenten[1].Length > 0)
                            Quartalsliste = komponenten[1].Split(';').ToList();
                        break;
                }
            }

            Pfad = string.Format("{0}_{1}",
                SchlechtdatenWahrscheinlichkeit == 0 && SchlechtdatenWahrscheinlichkeitFremdschlüssel == 0 ? "Gut" : "Schlecht", DateTime.Now.ToString("yyyy-MM-dd_HHmmssz"));
            if (!Directory.Exists(Konfiguration.Pfad))
                Directory.CreateDirectory(Konfiguration.Pfad);
        }

        public static string Dateiname
        {
            get { return dateiname;  }
            set
            {
                dateiname = value;

                foreach (string name in Schlüsselverzeichnismanager.Schlüsselverzeichnisnamen)
                    if (dateiname.Contains(string.Format("{{{0}}}", name)))
                        Dateiattribute.Add(name, Schlüsselverzeichnismanager.AlleEinträge(name));

                if (Dateiattribute.Any())
                {
                    DateiattributeKombinationenBilden(0, new Dictionary<string, string>());
                }
            }
        }

        // Oh nein, Hilfe, Rekursion!
        private static void DateiattributeKombinationenBilden(int attributzahl, Dictionary<string, string> attribute)
        {
            // Rekursionsabbruchbedingung, wir sind auf der Ebene des letzten Attributs
            if (attributzahl + 1 == Dateiattribute.Keys.Count)
            {
                List<Dictionary<string, string>> kombinationen = new List<Dictionary<string, string>>();
                foreach (string neuerSchlüssel in Dateiattribute[Dateiattribute.Keys.OrderBy(m => m).Skip(attributzahl).First()])
                {
                    Dictionary<string, string> kombination = new Dictionary<string, string>();

                    foreach (string alterSchlüssel in attribute.Keys)
                    {
                        kombination.Add(alterSchlüssel, attribute[alterSchlüssel]);
                    }

                    kombination.Add(Dateiattribute.Keys.OrderBy(m => m).Skip(attributzahl).First(), neuerSchlüssel);

                    DateiattributeKombinationen.Add(kombination);
                }
            }
            // Ansonsten den aktuellen Wert holen und für den ein Attribut tiefer springen
            else
            {
                string schlüssel = Dateiattribute.Keys.OrderBy(m => m).Skip(attributzahl).First();

                foreach (string wert in Dateiattribute[schlüssel])
                {
                    if (attribute.Keys.Contains(schlüssel))
                        attribute.Remove(schlüssel);

                    attribute.Add(schlüssel, wert);
                    DateiattributeKombinationenBilden(attributzahl + 1, attribute);
                }
            }
        }
    }
}
