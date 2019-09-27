using System;
using System.Collections.Generic;

namespace GTAVehicles.Models
{
    public partial class GtaplayerVehicles
    {
        public GtaplayerVehicles()
        {
            GtaplayerVehicleMods = new HashSet<GtaplayerVehicleMods>();
        }

        public int Id { get; set; }
        public int? VehicleId { get; set; }
        public int? PlayerId { get; set; }
        public int? PlayerGarageId { get; set; }

        public virtual Gtaplayers Player { get; set; }
        public virtual GtaplayerGarages PlayerGarage { get; set; }
        public virtual Gtavehicles Vehicle { get; set; }
        public virtual ICollection<GtaplayerVehicleMods> GtaplayerVehicleMods { get; set; }
    }
}