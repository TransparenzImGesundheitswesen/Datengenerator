using System;
using System.IO;

namespace Datengenerator.Loggen
{
    static class StringExtension
    {
        public static string ZeitstempelAnhängen(this string dateiname)
        {
            return string.Concat(
                Path.GetFileNameWithoutExtension(dateiname),
                    DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss-fff_z"),
                    Path.GetExtension(dateiname)
                );
        }
    }
}
