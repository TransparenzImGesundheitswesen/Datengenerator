using System.Collections.Generic;

namespace Datengenerator.Kern
{
    class Schlüsselverzeichnis
    {
        public string Name;
        public List<string> Einträge;

        public Schlüsselverzeichnis(string name)
        {
            Name = name;
            Einträge = new List<string>(System.IO.File.ReadAllLines(string.Format("XML-Testdateien/{0}.csv", Name)));
        }
    }
}
