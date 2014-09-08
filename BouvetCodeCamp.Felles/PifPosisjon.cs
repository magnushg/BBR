namespace BouvetCodeCamp.Felles
{
    public class PifPosisjon
    {
        public int LagId { get; set; }

        public string Latitude { get; set; }

        public string Longitude { get; set; }

        public PifPosisjon()
        {
            LagId = 0;
            Latitude = string.Empty;
            Longitude = string.Empty;
        }
    }
}