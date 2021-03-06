﻿using Datengenerator.Konfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Datengenerator.Kern
{
    class Datei
    {
        private string satzartname;
        private string dateiname;
        private XElement satzartXml;
        private List<string> primärschlüsselfelder;
        private List<string> primärschlüssel = new List<string>();
        public bool HatFremdschlüssel;
        private List<string> fremdschlüsselfelder;
        private Dictionary<string, string> dateiattribute;

        private Random r = Konfiguration.Reproduzierbar ? new Random(1) : new Random();
        private RandomProportional rp = Konfiguration.Reproduzierbar ? new RandomProportional(1) : new RandomProportional();

        private int zeilennummer = 0;

        public Datei(XElement satzartXml, List<Datei> alleDateien, Dictionary<string, string> dateiattribute, string rsn)
        {
            this.satzartXml = satzartXml;

            satzartname = this.satzartXml.Attribute("Name").Value;

            if (satzartXml.Descendants("Primärschlüssel").Descendants("Feld").Any())
                primärschlüsselfelder = satzartXml.Descendants("Primärschlüssel").Descendants("Feld").Select(m => m.Value).ToList();

            HatFremdschlüssel = satzartXml.Descendants("Fremdschlüssel").Any();

            if (HatFremdschlüssel && alleDateien.Count > 0)
            {
                string fremdschlüsselsatzart = satzartXml.Descendants("Fremdschlüssel").Descendants("Satzart").Select(m => m.Value).First();
                alleDateien.Where(m => m.satzartname == fremdschlüsselsatzart).First().ZeileGeneriert += ZeileGenerieren;

                fremdschlüsselfelder = satzartXml.Descendants("Fremdschlüssel").Descendants("Feld").Select(m => m.Value).ToList();
            }

            //Dateiname = string.Format("{0}_.{1}", Satzartname, Endung).ZeitstempelAnhängen();
            dateiname = Konfiguration.Dateiname;
            dateiname = dateiname.Replace("{Satzart}", satzartname);

            this.dateiattribute = dateiattribute;

            foreach (string attribut in dateiattribute.Keys)
            {
                string att = dateiattribute[attribut];

                if (Konfiguration.Auffüllen.Where(m => m.Attribut == attribut).Any())
                    while (att.Length < Konfiguration.Auffüllen.Where(m => m.Attribut == attribut).First().Länge)
                        att += Konfiguration.Auffüllen.Where(m => m.Attribut == attribut).First().Zeichen;

                dateiname = dateiname.Replace(string.Format("{{{0}}}", attribut), att);
            }

            if (rsn != null)
                dateiname = string.Format("{0}.", rsn) + dateiname;
            if (Konfiguration.LieferantenIK != null)
                dateiname += string.Format(".{0}", Konfiguration.LieferantenIK);
        }

        public void Generieren()
        {
            while (zeilennummer < Konfiguration.AnzahlZeilen)
            {
                Dictionary<string, string> primärschlüssel = Generieren(null);

                OnZeileGeneriert(new ZeileGeneriertEventArgs(primärschlüssel));

                if (zeilennummer % 1 == 0)
                {
                    string ausgabe = "";
                    foreach (string schlüssel in dateiattribute.Keys)
                        ausgabe += string.Format("{0}: {1}, ", schlüssel, dateiattribute[schlüssel]);
                    ausgabe += zeilennummer;
                    Console.WriteLine(ausgabe);
                }
            }
        }

        public event EventHandler ZeileGeneriert;
        protected virtual void OnZeileGeneriert(ZeileGeneriertEventArgs e)
        {
            //if (ZeileGeneriert != null)
            //{
            ZeileGeneriert?.Invoke(this, e);
            //    var eventListeners = ZeileGeneriert.GetInvocationList();

            //    for (int index = 0; index < eventListeners.Count(); index++)
            //    {
            //        var methode = (EventHandler)eventListeners[index];
            //        methode.BeginInvoke(this, e, EndAsyncEvent, null);
            //    }
            //}
        }

        private void EndAsyncEvent(IAsyncResult iar)
        {
            var ar = (System.Runtime.Remoting.Messaging.AsyncResult)iar;
            var invokedMethod = (EventHandler)ar.AsyncDelegate;

            try
            {
                invokedMethod.EndInvoke(iar);
            }
            catch
            {
                Console.WriteLine("EventListener fehlgeschlagen");
            }
        }

        void ZeileGenerieren(object sender, EventArgs e)
        {
            ZeileGeneriertEventArgs ev = (ZeileGeneriertEventArgs)e;
            ZeileGenerieren(ev.Primärschlüssel);
        }

        void ZeileGenerieren(Dictionary<string, string> fremdschlüssel)
        {
            Dictionary<string, string> primärschlüssel = Generieren(fremdschlüssel);

            OnZeileGeneriert(new ZeileGeneriertEventArgs(primärschlüssel));
        }

        private Dictionary<string, string> Generieren(Dictionary<string, string> fremdschlüssel)
        {
            string primärschlüsselString = "";
            Zeile zeile;
            string zeileString;
            Dictionary<string, string> primärschlüssel = new Dictionary<string, string>();

            do
            {
                zeile = new Zeile(satzartXml.Element("Felder"), r, rp, dateiattribute);
                zeileString = zeile.Generieren(fremdschlüssel, dateiname, zeilennummer);
                primärschlüsselString = "";

                if (primärschlüsselfelder != null)
                    foreach (string feld in primärschlüsselfelder)
                    {
                        primärschlüsselString += zeile.Feldliste[feld];
                        primärschlüssel.Add(feld, zeile.Feldliste[feld]);
                    }
            } while (primärschlüsselfelder != null && this.primärschlüssel.Contains(primärschlüsselString) && !(Konfiguration.SchlechtdatenWahrscheinlichkeit != 0 && r.Next(0, Konfiguration.SchlechtdatenWahrscheinlichkeit) == 0));

            if (primärschlüsselfelder != null)
                this.primärschlüssel.Add(primärschlüsselString);

            Schreiber.Schreiben(dateiname, zeileString);
            zeilennummer++;
            return primärschlüssel;
        }
    }
}
