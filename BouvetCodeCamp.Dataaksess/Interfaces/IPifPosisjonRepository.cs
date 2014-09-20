using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;

namespace BouvetCodeCamp.Dataaksess.Interfaces
{
    using Felles.Entiteter;

    public interface IPifPosisjonRepository : IRepository<PifPosisjon>
    {
        Task<IEnumerable<PifPosisjon>> HentPifPosisjonerForLag(string lagId);
    }

 
}