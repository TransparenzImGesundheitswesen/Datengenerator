
namespace Datengenerator.Loggen
{
    static class Logger
    {
        static string dateiname = "Datengenerator_Log_.txt".ZeitstempelAnhängen();

        public static void Loggen(string eintrag)
        {
            using (System.IO.StreamWriter datei = new System.IO.StreamWriter(dateiname, true))
            {
                datei.WriteLine(eintrag);
            }
        }
    }
}
