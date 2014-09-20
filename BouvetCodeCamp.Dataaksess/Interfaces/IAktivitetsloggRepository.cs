using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;

namespace BouvetCodeCamp.Dataaksess.Interfaces
{
    using BouvetCodeCamp.Felles.Entiteter;

    public interface IAktivitetsloggRepository : IRepository<AktivitetsloggHendelse>
    {
        
    }

   
}