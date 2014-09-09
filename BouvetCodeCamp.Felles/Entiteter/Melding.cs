namespace BouvetCodeCamp.Felles.Entiteter
{
    using System;

    public class Melding
    {
        public int LagId { get; set; }

        public DateTime Tid { get; set; }

        public MeldingsType Type { get; set; }

        public string Tekst { get; set; }

        public Melding()
        {
            this.LagId = 0;
            this.Tid = DateTime.MinValue;
            this.Type = MeldingsType.Ukjent;
            this.Tekst = string.Empty;
        }
    }
}