using System;
using System.Collections.Generic;

namespace GTAVehicles.Models
{
    public partial class GtaplayerGarages
    {
        public GtaplayerGarages()
        {
            GtaplayerVehicles = new HashSet<GtaplayerVehicles>();
        }

        public int Id { get; set; }
        public int? GarageId { get; set; }
        public int? CharacterId { get; set; }
        public string GarageName { get; set; }

        public virtual GtaplayerCharacters Character { get; set; }
        public virtual Gtagarages Garage { get; set; }
        public virtual ICollection<GtaplayerVehicles> GtaplayerVehicles { get; set; }
    }
}