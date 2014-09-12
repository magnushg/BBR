using System.Collections.Generic;
using System.Threading.Tasks;

namespace BouvetCodeCamp.Dataaksess.Interfaces
{
    using Felles.Entiteter;

    public interface IPifPosisjonRepository : IRepository<PifPosisjon>
    {
        Task<IEnumerable<PifPosisjon>> HentPifPosisjon(string lagId);
    }
}