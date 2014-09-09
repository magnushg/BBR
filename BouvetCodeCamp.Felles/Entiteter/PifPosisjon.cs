namespace BouvetCodeCamp.Felles.Entiteter
{
    using System;

    public class PifPosisjon
    {
        public int LagId { get; set; }

        public string Latitude { get; set; }

        public string Longitude { get; set; }

        public DateTime Tid { get; set; }

        public PifPosisjon()
        {
            this.LagId = 0;
            this.Latitude = string.Empty;
            this.Longitude = string.Empty;
            this.Tid = DateTime.MinValue;
        }
    }
}