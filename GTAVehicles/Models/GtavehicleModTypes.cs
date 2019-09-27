using System;
using System.Collections.Generic;

namespace GTAVehicles.Models
{
    public partial class GtavehicleModTypes
    {
        public GtavehicleModTypes()
        {
            GtaplayerVehicleMods = new HashSet<GtaplayerVehicleMods>();
        }

        public int Id { get; set; }
        public string ModType { get; set; }

        public virtual ICollection<GtaplayerVehicleMods> GtaplayerVehicleMods { get; set; }
    }
}