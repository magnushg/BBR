namespace BouvetCodeCamp.DomeneTjenester.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using BouvetCodeCamp.Domene.Entiteter;
    using BouvetCodeCamp.DomeneTjenester.Interfaces;

    public abstract class Service<T> : IService<T> where T : BaseDocument
    {
        private readonly IRepository<T> _repository;

        protected Service(IRepository<T> repository)
        {
            _repository = repository;
        }

        public IEnumerable<T> HentAlle()
        {
            return _repository.HentAlle();
        }

        public async Task Oppdater(T entitet)
        {
            await _repository.Oppdater(entitet);
        }

        public virtual T Hent(string id)
        {
            return _repository.Hent(id);
        }

        public async Task SlettAlle()
        {
            await _repository.SlettAlle();
        }

        public async Task Slett(T entitet)
        {
            await _repository.Slett(entitet);
        }

        public Task Opprett(T entitet)
        {
            return _repository.Opprett(entitet);
        }

        public IEnumerable<T> Søk(Func<T, bool> predicate)
        {
            return _repository.Søk(predicate);
        }
    }
}