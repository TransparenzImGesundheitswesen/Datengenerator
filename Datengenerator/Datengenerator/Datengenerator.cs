﻿using System;

using Datengenerator.Konfig;
using Datengenerator.Loggen;
using Datengenerator.XML;

namespace Datengenerator
{
    class Datengenerator
    {
        static void Main(string[] args)
        {
            foreach (string xsd in Konfiguration.Xsd)
                Logger.Loggen(string.Format("XSD-Datei: {0}", xsd));
            Logger.Loggen(string.Format("XML-Datei: {0}", Konfiguration.Xml));
            Logger.Loggen(string.Format("Validieren: {0}", Konfiguration.Validieren ? "ja" : "nein"));

            if (Konfiguration.Validieren)
            {
                var validierer = new XsdValidierer();

                foreach (string xsd in Konfiguration.Xsd)
                    validierer.SchemaHinzufügen(xsd);
                var istValide = validierer.IstValide(Konfiguration.Xml);

                Console.WriteLine(istValide);

                Logger.Loggen(string.Format("Ist valide: {0}", istValide ? "ja" : "nein"));
            }

            Console.WriteLine("Narf!");
            Console.ReadLine();
        }
    }
}
