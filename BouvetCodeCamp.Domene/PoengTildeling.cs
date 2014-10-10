namespace BouvetCodeCamp.Domene
{
    public class PoengTildeling
    {
        public int KodeOppdaget = 0;
        public int MeldingsStraff = 0;

        //alle konstanter er målt i sekund

        public int PingTimeout = 10;
        public int PingTimeoutStraff = 1; //skal dette ganges?

        public int InfisertTidssfrist = 5;
        public double InfisertTickStraff = 10; //poengtap per sekund etter tidsfrist

    }
}
