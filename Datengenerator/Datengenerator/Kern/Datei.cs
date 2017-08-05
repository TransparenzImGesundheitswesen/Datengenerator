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
        private XElement SatzartXml;
        private List<string> Primärschlüsselfelder;
        private List<string> Primärschlüssel = new List<string>();
        public bool HatFremdschlüssel;
        private List<string> Fremdschlüsselfelder;
        private Dictionary<string, string> dateiattribute;

        private Random r = new Random(1);
        private RandomProportional rp = new RandomProportional();

        public Datei(XElement satzartXml, List<Datei> alleDateien, Dictionary<string, string> dateiattribute)
        {
            SatzartXml = satzartXml;
            
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

            //Dateiname = string.Format("{0}_.{1}", Satzartname, Endung).ZeitstempelAnhängen();
            Dateiname = Konfiguration.Dateiname;
            Dateiname = Dateiname.Replace("{Satzart}", Satzartname);

            this.dateiattribute = dateiattribute;

            foreach (string attribut in dateiattribute.Keys)
                Dateiname = Dateiname.Replace(string.Format("{{{0}}}", attribut), dateiattribute[attribut]);
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

                    zeile = new Zeile(SatzartXml.Element("Felder"), r, rp, dateiattribute);
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

                if (i % 1000 == 0)
                {
                    string ausgabe = "";
                    foreach (string schlüssel in dateiattribute.Keys)
                        ausgabe += string.Format("{0}: {1}, ", schlüssel, dateiattribute[schlüssel]);
                    ausgabe += i;
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
            string primärschlüsselString = "";
            Zeile zeile;
            string zeileString;
            Dictionary<string, string> primärschlüssel = new Dictionary<string, string>();

            do
            {
                zeile = new Zeile(SatzartXml.Element("Felder"), r, rp, dateiattribute);
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
