using System;

namespace BouvetCodeCamp.Domene.OutputModels
{
    public class PifPosisjonModell
    {
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string LagId { get; set; }
        public DateTime? Tid { get; set; }
    }
}