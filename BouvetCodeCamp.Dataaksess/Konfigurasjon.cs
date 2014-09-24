namespace BouvetCodeCamp.Infrastruktur
{
    using System.Configuration;

    public class Konfigurasjon : IKonfigurasjon
    {
        public string HentAppSetting(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }

    public interface IKonfigurasjon
    {
        string HentAppSetting(string key);
    }
}
