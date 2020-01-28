using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GTAVehicles.Models
{
    public class GtacharactersDTO
    {
        public string Id { get; set; }
        public string UserName { get; set; }

        public List<GtaplayerGarages> GtaplayerGarages { get; set; }
    }
}
