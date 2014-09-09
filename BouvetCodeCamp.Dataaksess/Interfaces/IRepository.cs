namespace BouvetCodeCamp.Dataaksess.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IRepository<T>
    {
        Task Opprett(T entitet);

        Task<IEnumerable<T>> HentAlle();
    }
}