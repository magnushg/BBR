namespace BouvetCodeCamp.Felles.Entiteter
{
    using System;

    public class AktivitetsloggEntry
    {
        public int LagId { get; set; }

        public HendelsesType HendelsesType { get; set; }

        public DateTime Tid { get; set; }

        public AktivitetsloggEntry()
        {
            LagId = 0;
            HendelsesType = HendelsesType.Ukjent;
            Tid = DateTime.MinValue;
        }
    }
}