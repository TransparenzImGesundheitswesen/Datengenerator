using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Datengenerator.Kern
{
    static class Schlüsselverzeichnismanager
    {
        private static List<Schlüsselverzeichnis> Schlüsselverzeichnisse;
        private static Random random;

        public static List<string> Schlüsselverzeichnisnamen;

        static Schlüsselverzeichnismanager()
        {
            Schlüsselverzeichnisse = new List<Schlüsselverzeichnis>();
            Schlüsselverzeichnisnamen = new List<string>();
            random = new Random();
        }

        public static void SchlüsselverzeichnisseHinzufügen(IEnumerable<XElement> schlüsselverzeichnisseXml)
        {
            foreach (XElement schlüsselverzeichnisXml in schlüsselverzeichnisseXml.Descendants("Schlüsselverzeichnis"))
            {
                Schlüsselverzeichnis sv = new Schlüsselverzeichnis(schlüsselverzeichnisXml.Value);
                Schlüsselverzeichnisse.Add(sv);
                Schlüsselverzeichnisnamen.Add(sv.Name);
            }
        }

        public static string ZufälligerEintrag(string name)
        {
            Schlüsselverzeichnis sv = Schlüsselverzeichnisse.Where(m => m.Name == name).First();

            return sv.Einträge[random.Next(sv.Einträge.Count)];
        }

        public static List<string> AlleEinträge(string name)
        {
            return Schlüsselverzeichnisse.Where(m => m.Name == name).First().Einträge;
        }
    }
}
