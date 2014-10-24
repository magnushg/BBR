using BouvetCodeCamp.Domene;
using BouvetCodeCamp.Domene.Entiteter;

namespace BouvetCodeCamp.DomeneTjenester.Interfaces
{
    public interface IPoengService
    {
        //Straffemetoder
        Lag SjekkOgSettPifPingStraff(Lag lag);
        Lag SjekkOgSettInfisertSoneStraff(Lag lag);

        Lag SettFritekstMeldingSendtStraff(Lag lag, Melding melding);

        //Poengtildeling
        Lag SettPoengForKodeRegistrert(Lag lag, HendelseType hendelse, int postnummer);

        Lag SettPoengForLag(Lag lag, int poeng, string kommentar);
    }
}
