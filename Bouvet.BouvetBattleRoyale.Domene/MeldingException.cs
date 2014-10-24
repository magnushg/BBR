namespace Bouvet.BouvetBattleRoyale.Domene
{
    using System;

    public class MeldingException : Exception
    {
        public MeldingException(string melding) : base(melding) { }
    }
}
