using Datengenerator.Konfig;
using Datengenerator.Loggen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Datengenerator.Kern
{
    class Datei
    {
        private string Satzartname;
        private string Dateiname;
        private string Zeichensatz;
        private string Feldtrennzeichen;
        private string Zeilentrennzeichen;
        private string Endung;
        private XElement SatzartXml;
        private List<string> Primärschlüsselfelder;
        private List<string> Primärschlüssel = new List<string>();
        public bool HatFremdschlüssel;
        private List<string> Fremdschlüsselfelder;

        private Random r = new Random();
        private RandomProportional rp = new RandomProportional();

        public Datei(XElement satzartXml, List<Datei> alleDateien)
        {
            SatzartXml = satzartXml;

            Feldtrennzeichen = "#";
            Zeilentrennzeichen = "\r\n";
            Endung = "csv";
            Satzartname = SatzartXml.Attribute("Name").Value;

            if (SatzartXml.Descendants("Primärschlüssel").Descendants("Feld").Any())
                Primärschlüsselfelder = SatzartXml.Descendants("Primärschlüssel").Descendants("Feld").Select(m => m.Value).ToList();

            HatFremdschlüssel = SatzartXml.Descendants("Fremdschlüssel").Any();

            if (HatFremdschlüssel && alleDateien.Count > 0)
            {
                string fremdschlüsselsatzart = SatzartXml.Descendants("Fremdschlüssel").Descendants("Satzart").Select(m => m.Value).First();
                alleDateien.Where(m => m.Satzartname == fremdschlüsselsatzart).First().ZeileGeneriert += ZeileGenerieren;

                Fremdschlüsselfelder = SatzartXml.Descendants("Fremdschlüssel").Descendants("Feld").Select(m => m.Value).ToList();
            }

            Dateiname = string.Format("{0}_.{1}", Satzartname, Endung).ZeitstempelAnhängen();
        }

        public void Generieren()
        {
            Dictionary<string, string> primärschlüssel = new Dictionary<string, string>();

            for (int i = 0; i < Konfiguration.AnzahlZeilen; i++)
            {
                string primärschlüsselString = "";
                Zeile zeile;
                string zeileString;

                do
                {
                    primärschlüssel.Clear();

                    zeile = new Zeile(Feldtrennzeichen, Zeilentrennzeichen, SatzartXml.Element("Felder"), r, rp);
                    zeileString = zeile.Generieren(null);
                    primärschlüsselString = "";

                    if (Primärschlüsselfelder != null)
                        foreach (string feld in Primärschlüsselfelder)
                        {
                            primärschlüsselString += zeile.Feldliste[feld];
                            primärschlüssel.Add(feld, zeile.Feldliste[feld]);
                        }
                } while (Primärschlüsselfelder != null && Primärschlüssel.Contains(primärschlüsselString) && !(Konfiguration.SchlechtdatenWahrscheinlichkeit != 0 && r.Next(0, Konfiguration.SchlechtdatenWahrscheinlichkeit) == 0));

                if (Primärschlüsselfelder != null)
                    Primärschlüssel.Add(primärschlüsselString);

                Schreiber.Schreiben(Dateiname, zeileString);

                OnZeileGeneriert(new ZeileGeneriertEventArgs(primärschlüssel));

                Console.WriteLine(i);
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
            string primärschlüsselString = "";
            Zeile zeile;
            string zeileString;
            Dictionary<string, string> primärschlüssel = new Dictionary<string, string>();

            do
            {
                zeile = new Zeile(Feldtrennzeichen, Zeilentrennzeichen, SatzartXml.Element("Felder"), r, rp);
                zeileString = zeile.Generieren(fremdschlüssel);
                primärschlüsselString = "";

                if (Primärschlüsselfelder != null)
                    foreach (string feld in Primärschlüsselfelder)
                    {
                        primärschlüsselString += zeile.Feldliste[feld];
                        primärschlüssel.Add(feld, zeile.Feldliste[feld]);
                    }
            } while (Primärschlüsselfelder != null && Primärschlüssel.Contains(primärschlüsselString) && !(Konfiguration.SchlechtdatenWahrscheinlichkeit != 0 && r.Next(0, Konfiguration.SchlechtdatenWahrscheinlichkeit) == 0));

            if (Primärschlüsselfelder != null)
                Primärschlüssel.Add(primärschlüsselString);

            Schreiber.Schreiben(Dateiname, zeileString);

            OnZeileGeneriert(new ZeileGeneriertEventArgs(primärschlüssel));
        }
    }
}
