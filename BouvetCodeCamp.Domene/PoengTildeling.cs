namespace BouvetCodeCamp.Domene
{
    public class PoengTildeling
    {
        public static int KodeOppdaget = 0;
        public static int MeldingsStraff = 0;

        //alle konstanter er målt i sekund

        public static int PingTimeout = 10;
        public static int PingTimeoutStraff = 1; //skal dette ganges?

        public static int InfisertTidssfrist = 5;
        public static double InfisertTickStraff = 10; //poengtap per sekund etter tidsfrist
    }
}