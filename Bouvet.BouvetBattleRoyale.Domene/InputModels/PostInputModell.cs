namespace Bouvet.BouvetBattleRoyale.Domene.InputModels
{
    using Bouvet.BouvetBattleRoyale.Domene.Entiteter;

    public class PostInputModell
    {
        public string Kode { get; set; }
        public int Postnummer { get; set; }
        public string LagId { get; set; }
        public Koordinat Koordinat { get; set; }

        public PostInputModell()
        {
            Kode = string.Empty;
            Postnummer = 0;
            LagId = string.Empty;
            Koordinat = Koordinat.Empty;
        }
    }
}
