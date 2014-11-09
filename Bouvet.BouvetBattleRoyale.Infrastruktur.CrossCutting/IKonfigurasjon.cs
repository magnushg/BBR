namespace Bouvet.BouvetBattleRoyale.Infrastruktur.CrossCutting
{
    public interface IKonfigurasjon
    {
        string HentAppSetting(string key);
    }
}