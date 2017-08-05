using Datengenerator.Konfig;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Datengenerator.Kern
{
    public static class Schreiber
    {
        private static List<Tuple<string, StreamWriter>> dateien = new List<Tuple<string, StreamWriter>>();
        private static Object theLock = new Object();

        public static void Schreiben(string dateiname, string zeile)
        {
            lock (theLock)
            {
                if (!dateien.Where(m => m.Item1 == dateiname).Any())
                    dateien.Add(new Tuple<string, StreamWriter>(dateiname, new StreamWriter(
                        string.Format("{0}/{1}", Konfiguration.Pfad, dateiname), false, Encoding.GetEncoding(Konfiguration.Zeichensatz))));

                dateien.Where(m => m.Item1 == dateiname).First().Item2.Write(zeile);
                dateien.Where(m => m.Item1 == dateiname).First().Item2.Flush();
            }
        }
    }
}
