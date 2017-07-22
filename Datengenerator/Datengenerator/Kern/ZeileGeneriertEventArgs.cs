using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datengenerator.Kern
{
    class ZeileGeneriertEventArgs : EventArgs
    {
        public int SchlechtdatenWahrscheinlichkeit;
        public Dictionary<string, string> Primärschlüssel;

        public ZeileGeneriertEventArgs(int schlechtdatenWahrscheinlichkeit, Dictionary<string, string> primärschlüssel) : base()
        {
            SchlechtdatenWahrscheinlichkeit = schlechtdatenWahrscheinlichkeit;
            Primärschlüssel = primärschlüssel;
        }
    }
}
