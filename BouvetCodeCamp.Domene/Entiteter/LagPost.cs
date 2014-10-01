using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BouvetCodeCamp.Domene.Entiteter
{
    public class LagPost : Post
    {
        [JsonProperty(PropertyName = "tilstand")]
        public PostTilstand PostTilstand { get; set; }

        [JsonProperty(PropertyName = "kode")]
        public string Kode { get; set; }

        [JsonProperty(PropertyName = "sekvensnummer")]
        public int Sekvensnummer { get; set; }
    }
}
