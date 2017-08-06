using System;
using System.Collections.Generic;
using System.Linq;

using Datengenerator.Kern;
using Datengenerator.Konfig;

namespace Datengenerator.Loggen
{
    static class Logger
    {
        private static string dateiname = "Datengenerator_Log_.txt".ZeitstempelAnhängen();
        private static string dateiname_schlechtfelder = "Schlechtfelder_Log_.txt".ZeitstempelAnhängen();
        private static Object theLock = new Object();
        private static List<Schlechtfeld> schlechtfelder = new List<Schlechtfeld>();


        public static void Loggen(string eintrag)
        {
            lock (theLock)
            {
                using (System.IO.StreamWriter datei = new System.IO.StreamWriter(string.Format("{0}/{1}", Konfiguration.Pfad, dateiname), true))
                {
                    datei.WriteLine(string.Format("{0} {1}", DateTime.Now.ToString("yyyy-MM-dd_HHmmssz"), eintrag));
                }
            }
        }

        public static void LoggenSchlechtfeld(Schlechtfeld schlechtfeld)
        {
            schlechtfelder.Add(schlechtfeld);
        }

        public static void SchlechtfelderSpeichern()
        {
            using (System.IO.StreamWriter datei = new System.IO.StreamWriter(string.Format("{0}/{1}", Konfiguration.Pfad, dateiname_schlechtfelder), true))
            {
                foreach (Schlechtfeld sf in schlechtfelder.OrderBy(m => m.Dateiname).ThenBy(m => m.Zeile).ThenBy(m => m.Feld))
                    datei.WriteLine(string.Format("{0}#{1}#{2}", sf.Dateiname, sf.Zeile, sf.Feld));
            }
        }
    }
}
