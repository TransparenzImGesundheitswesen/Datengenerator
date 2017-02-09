using System;

namespace Datengenerator.Kern
{
    public class RandomProportional : Random
    {
        // Generiert eine Verteilung proportional zum Wert 
        // der Zufallszahl, im Intervall [0.0, 1.0].
        protected override double Sample()
        {
            return Math.Sqrt(base.Sample());
        }

        public override int Next(int maxValue)
        {
            return (int)(Sample() * maxValue);
        }
    }
}
