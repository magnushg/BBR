namespace Bouvet.BouvetBattleRoyale.Infrastruktur.CrossCutting
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;

    public class Konfigurasjon : IKonfigurasjon
    {
        private readonly Dictionary<string, string> _settings = new Dictionary<string, string>();

        public string HentAppSetting(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("key var null eller tom");

            lock (key)
            {
                if (!_settings.ContainsKey(key))
                {
                    var setting = HentSetting(key);

                    if (setting == null)
                    {
                        throw new ConfigurationErrorsException(string.Format("{0} setting mangler", key));
                    }

                    if (setting.Length == 0)
                    {
                        throw new ConfigurationErrorsException(string.Format("{0} setting er tom", key));
                    }
                    
                    _settings.Add(key, setting);
                }
            }
            return _settings[key];
        }

        /// <summary>
        /// Legger opp til at en ikke trenger å publisere url og nøkler til Azure-kontoer ved
        /// å sjekke om setting er overstyrt i Environment-variabel. På denne måten kan en utvikler kjøre
        /// på privat Azure-konto.
        /// </summary>
        private string HentSetting(string key)
        {
            var userValue = Environment.GetEnvironmentVariable(key, EnvironmentVariableTarget.User);

            if (!string.IsNullOrEmpty(userValue))
                return userValue;

            var machineValue = Environment.GetEnvironmentVariable(key, EnvironmentVariableTarget.Machine);

            if (!string.IsNullOrEmpty(machineValue))
                return machineValue;
            
            return ConfigurationManager.AppSettings[key];
        }
    }
}