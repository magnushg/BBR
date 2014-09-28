using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BouvetCodeCamp.Domene;
using BouvetCodeCamp.Domene.Entiteter;
using BouvetCodeCamp.Infrastruktur.DataAksess;
using BouvetCodeCamp.Infrastruktur.DataAksess.Repositories;
using Newtonsoft.Json;

namespace BouvetCodeCamp.SpillOppretter
{
    public class LagOppretter
    {
        private readonly LagRepository _lagRepository;

        public LagOppretter()
        {
            _lagRepository = new LagRepository(new Konfigurasjon(), new DocumentDbContext(new Konfigurasjon()));
        }

        public void OpprettLag()
        {
            var lagJson = File.ReadAllText("importData/lagData.json", Encoding.UTF8);

            LagreLagliste(JsonConvert.DeserializeObject<IEnumerable<Lag>>(lagJson));
        }

        public void LagreLagliste(IEnumerable<Lag> lagListe)
        {
            _lagRepository.SlettAlle();
            lagListe.ToList().ForEach(async x =>
            {
                await _lagRepository.Opprett(x);
            });
        }
    }
}
