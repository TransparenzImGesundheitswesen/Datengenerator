using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datengenerator.Kern
{
    class ZeileGeneriertEventArgs : EventArgs
    {
        public Dictionary<string, string> Primärschlüssel;

        public ZeileGeneriertEventArgs(Dictionary<string, string> primärschlüssel) : base()
        {
            Primärschlüssel = primärschlüssel;
        }
    }
}
