namespace Bouvet.BouvetBattleRoyale.Domene
{
    using System;

    public class ConcurrencyException : Exception
    {
        public ConcurrencyException(string melding)
            : base(melding)
        {
        }
    }
}