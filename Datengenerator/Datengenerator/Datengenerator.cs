using System;

using Datengenerator.XML;

namespace Datengenerator
{
    class Datengenerator
    {
        static void Main(string[] args)
        {
            var validierer = new XsdValidierer();
            validierer.SchemaHinzufügen(@"XML-Testdateien/ASV.xsd");
            var istValide = validierer.IstValide(@"XML-Testdateien/ASV.xml");
            Console.WriteLine(istValide);

            Console.WriteLine("Narf!");
            Console.ReadLine();
        }
    }
}
