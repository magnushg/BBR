namespace Bouvet.BouvetBattleRoyale.Domene
{
    public class PoengTildeling
    {
        public static int KodeOppdaget = 1000;
        public static int MeldingsStraff = -10;

        //alle konstanter er målt i sekund

        public static int PingTimeout = 1000;
        public static int PingTimeoutStraff = 0; //skal dette ganges?

        public static int InfisertTidssfrist = 5;
        public static double InfisertTickStraff = -10; //poengtap per sekund etter tidsfrist
    }
}