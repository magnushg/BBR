using System.Configuration;

namespace BouvetCodeCamp.Felles.Konfigurasjon
{
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
