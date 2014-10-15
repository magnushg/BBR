namespace BouvetCodeCamp.Domene
{
    public class HendelseTypeFormatter
    {
        public static string HentTekst(HendelseType hendelseType)
        {
            var beskrivelse = string.Empty;

            switch (hendelseType)
            {
                case HendelseType.HentetPifPosisjon:
                    beskrivelse = "Hentet PIF-posisjon";
                    break;

                case HendelseType.RegistrertKodeMislykket:
                    beskrivelse = "Registrere kode mislyktes";
                    break;
                    
                case HendelseType.RegistrertKodeSuksess:
                    beskrivelse = "Kode registrert";
                    break;

                case HendelseType.RegistrertPifPosisjon:
                    beskrivelse = "Registrert PIF-posisjon";
                    break;

                case HendelseType.SendtMelding:
                    beskrivelse = "Melding sendt";
                    break;

                case HendelseType.TildeltPoeng:
                    beskrivelse = "Tildelt poeng";
                    break;

                case HendelseType.Ukjent:
                    beskrivelse = "Ukjent";
                    break;

                case HendelseType.Achievement:
                    beskrivelse = "Achievement nådd";
                    break;

                case HendelseType.SendtFritekstmeldingStraff:
                    beskrivelse = "Sendt fritekstmeldingstraff";
                    break;
                
                case HendelseType.InfisertPoengTap:
                    beskrivelse = "Infisert, poengtap";
                    break;

                case HendelseType.PingPoengTap:
                    beskrivelse = "Treg ping, poengtap";
                    break;
            }

            return beskrivelse;
        }
    }
}