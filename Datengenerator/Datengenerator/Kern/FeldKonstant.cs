﻿using System;
using System.Xml.Linq;

namespace Datengenerator.Kern
{
    class FeldKonstant : Feld
    {
        public string Konstant;

        public FeldKonstant(XElement xml, Random r) : base(xml, r)
        {
            Konstant = xml.Element("Konstant").Value;
        }

        public override string Generieren()
        {
            return Konstant;
        }
    }
}
