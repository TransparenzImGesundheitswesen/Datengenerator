﻿using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Datengenerator.Kern
{
    class FeldKV : Feld
    {
        public List<string> KVen = new List<string> { "01", "02", "03", "17", "20", "38", "46", "51", "52", "71", "72", "73", "78", "83", "88", "93", "98" };

        public FeldKV(XElement xml, Random r) : base(xml, r)
        {
        }

        public override string Generieren()
        {
            return KVen[Random.Next(KVen.Count)];
        }
    }
}