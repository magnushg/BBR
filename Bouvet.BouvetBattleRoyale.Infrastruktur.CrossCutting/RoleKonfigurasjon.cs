namespace Bouvet.BouvetBattleRoyale.Infrastruktur.CrossCutting
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;

    using Microsoft.WindowsAzure.ServiceRuntime;

    public class RoleKonfigurasjon : IKonfigurasjon
    {
        private readonly Dictionary<string, string> _settings = new Dictionary<string, string>();

        public string HentAppSetting(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("key var null eller tom");

            if (!_settings.ContainsKey(key))
            {
                var setting = HentSetting(key);

                if (setting == null)
                {
                    throw new ConfigurationErrorsException(string.Format("{0} rolesetting mangler", key));
                }

                if (setting.Length == 0)
                {
                    throw new ConfigurationErrorsException(string.Format("{0} rolesetting er tom", key));
                }

                _settings.Add(key, setting);
            }
            return _settings[key];
        }
        
        /// <summary>
        /// Leser setting fra ServiceConfiguration hvis man kjører i Azure eller i Azure-emulator.
        /// </summary>
        private string HentSetting(string key)
        {
            return RoleEnvironment.GetConfigurationSettingValue(key);
        }
    }
}