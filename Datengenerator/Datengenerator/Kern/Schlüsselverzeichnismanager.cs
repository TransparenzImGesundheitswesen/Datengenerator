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

        static Schlüsselverzeichnismanager()
        {
            Schlüsselverzeichnisse = new List<Schlüsselverzeichnis>();
            random = new Random();
        }

        public static void SchlüsselverzeichnisseHinzufügen(IEnumerable<XElement> schlüsselverzeichnisseXml)
        {
            foreach (XElement schlüsselverzeichnisXml in schlüsselverzeichnisseXml.Descendants("Schlüsselverzeichnis"))
                Schlüsselverzeichnisse.Add(new Schlüsselverzeichnis(schlüsselverzeichnisXml.Value));
        }

        public static string ZufälligerEintrag(string name)
        {
            Schlüsselverzeichnis sv = Schlüsselverzeichnisse.Where(m => m.Name == name).First();

            return sv.Einträge[random.Next(sv.Einträge.Count)];
        }
    }
}
