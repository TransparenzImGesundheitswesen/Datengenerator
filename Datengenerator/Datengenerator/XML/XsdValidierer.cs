using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml;
using System.Xml.Schema;

namespace Datengenerator.XML
{
    class XsdValidierer
    {
        public List<XmlSchema> Schemata { get; set; }
        public List<String> Fehler { get; set; }
        public List<String> Warnungen { get; set; }

        /// <summary>
        /// Konstruktor
        /// </summary>
        public XsdValidierer()
        {
            Schemata = new List<XmlSchema>();
        }

        /// <summary>
        /// Fügt ein Schema hinzu, das bei einer Validierung angewendet wird
        /// </summary>
        /// <param name="schemaDatei">Pfad und Name der Schemadatei</param>
        /// <returns>wahr, falls das Schema erfolgreich geladen werden konnte; falsch sonst, dann Fehler und Warnungen ansehen</returns>
        public bool SchemaHinzufügen(string schemaDatei)
        {
            if (String.IsNullOrEmpty(schemaDatei))
                throw new NullReferenceException("XSD-Datei muss angegeben werden.");
            if (!File.Exists(schemaDatei))
                throw new FileNotFoundException("Die XSD-Datei existiert nicht.", schemaDatei);

            // Reset the Error/Warning collections
            Fehler = new List<string>();
            Warnungen = new List<string>();

            XmlSchema schema;

            using (var fs = File.OpenRead(schemaDatei))
            {
                schema = XmlSchema.Read(fs, ValidationEventHandler);
            }

            var isValid = !Fehler.Any() && !Warnungen.Any();

            if (isValid)
                Schemata.Add(schema);

            return isValid;
        }

        /// <summary>
        /// Prüft eine XML-Datei gegen die Schemasammlung
        /// </summary>
        /// <param name="xmlDatei">Pfad und Name der XML-Datei</param>
        /// <returns>wahr, falls die Datei den Schemata entspricht; falsch sonst, dann Fehler und Warnungen ansehen</returns>
        public bool IstValide(string xmlDatei)
        {
            if (String.IsNullOrEmpty(xmlDatei))
                throw new NullReferenceException("XML-Datei muss angegeben werden.");
            if (!File.Exists(xmlDatei))
                throw new FileNotFoundException("Die XML-Datei existiert nicht.", xmlDatei);

            using (var xmlStream = File.OpenRead(xmlDatei))
            {
                return IstValide(xmlStream);
            }
        }

        /// <summary>
        /// Prüft einen XML-Stream gegen die Schemasammlung
        /// </summary>
        /// <param name="xmlStream">Stream der XML-Datei</param>
        /// <returns>wahr, falls die Datei den Schemata entspricht; falsch sonst, dann Fehler und Warnungen ansehen</returns>
        private bool IstValide(Stream xmlStream)
        {
            // Reset the Error/Warning collections
            Fehler = new List<string>();
            Warnungen = new List<string>();

            var settings = new XmlReaderSettings
            {
                ValidationType = ValidationType.Schema
            };
            settings.ValidationEventHandler += ValidationEventHandler;

            foreach (var xmlSchema in Schemata)
            {
                settings.Schemas.Add(xmlSchema);
            }

            var xmlFile = XmlReader.Create(xmlStream, settings);

            try
            {
                while (xmlFile.Read()) { }
            }
            catch (XmlException xex)
            {
                Fehler.Add(xex.Message);
            }

            return !Fehler.Any() && !Warnungen.Any();
        }

        /// <summary>
        /// Ereignishandler für Validierungsfehler
        /// </summary>
        /// <param name="sender">Sender des Ereignisses</param>
        /// <param name="e">Argumente (ValidationEventArgs)</param>
        private void ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            switch (e.Severity)
            {
                case XmlSeverityType.Error:
                    Fehler.Add(e.Message);
                    break;
                case XmlSeverityType.Warning:
                    Warnungen.Add(e.Message);
                    break;
            }
        }
    }
}
