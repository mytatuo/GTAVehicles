using System;
using System.Collections.Generic;

namespace GTAVehicles.Models
{
    public partial class GtaplayerVehicleMods
    {
        public int Id { get; set; }
        public int VehicleOwnedId { get; set; }
        public int ModTypeId { get; set; }
        public string ModVariation { get; set; }
        public bool Incomplete { get; set; }

        public virtual GtavehicleModTypes ModType { get; set; }
        public virtual GtaplayerVehicles VehicleOwned { get; set; }
    }
}