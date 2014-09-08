namespace BouvetCodeCamp.Dataaksess.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using BouvetCodeCamp.Felles;

    public interface IRepository<T>
    {
        Task Opprett(T entitet);

        Task<IEnumerable<PifPosisjon>> HentAlle();
    }
}