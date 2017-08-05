using Datengenerator.Konfig;

namespace Datengenerator.Loggen
{
    static class Logger
    {
        static string dateiname = "Datengenerator_Log_.txt".ZeitstempelAnhängen();

        public static void Loggen(string eintrag)
        {
            using (System.IO.StreamWriter datei = new System.IO.StreamWriter(string.Format("{0}/{1}", Konfiguration.Pfad, dateiname), true))
            {
                datei.WriteLine(eintrag);
            }
        }
    }
}
