using System;

namespace BouvetCodeCamp.Domene
{
    public class MeldingException : Exception
    {
        public MeldingException(string melding) : base(melding) { }
    }
}
