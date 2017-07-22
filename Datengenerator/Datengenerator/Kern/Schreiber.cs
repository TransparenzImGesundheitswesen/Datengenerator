using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Datengenerator.Kern
{
    public static class Schreiber
    {
        static List<Tuple<string, StreamWriter>> dateien = new List<Tuple<string, StreamWriter>>();
        private static Object theLock = new Object();

        public static void Schreiben(string dateiname, string zeile)
        {
            lock (theLock)
            {
                if (!dateien.Where(m => m.Item1 == dateiname).Any())
                    dateien.Add(new Tuple<string, StreamWriter>(dateiname, new StreamWriter(dateiname, true, Encoding.GetEncoding("iso-8859-15"))));

                dateien.Where(m => m.Item1 == dateiname).First().Item2.Write(zeile);
                dateien.Where(m => m.Item1 == dateiname).First().Item2.Flush();
            }
        }
    }
}
