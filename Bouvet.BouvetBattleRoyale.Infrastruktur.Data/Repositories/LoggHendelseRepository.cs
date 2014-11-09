namespace Bouvet.BouvetBattleRoyale.Infrastruktur.Data.Repositories
{
    using System;

    using Bouvet.BouvetBattleRoyale.Domene.Entiteter;
    using Bouvet.BouvetBattleRoyale.Infrastruktur.CrossCutting;
    using Bouvet.BouvetBattleRoyale.Infrastruktur.Data.Interfaces;

    using log4net;

    public class LoggHendelseRepository : Repository<LoggHendelse>
    {
        private string _collectionId;

        public LoggHendelseRepository(
            IKonfigurasjon konfigurasjon, 
            IDocumentDbContext context, 
            ILog log) : base(konfigurasjon, context, log)
        {
        }

        public override String CollectionId
        {
            get
            {
                if (string.IsNullOrEmpty(_collectionId))
                {
                    _collectionId = _konfigurasjon.HentAppSetting(DocumentDbKonstanter.LoggHendelseCollectionId);
                }

                return _collectionId;
            }
        }
    }
}