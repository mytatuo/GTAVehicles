using System;
using System.Collections.Generic;

namespace GTAVehicles.Models
{
    public partial class Gtaplayers
    {
        public Gtaplayers()
        {
            GtaplayerVehicles = new HashSet<GtaplayerVehicles>();
        }

        public int Id { get; set; }
        public string UserName { get; set; }

        public virtual ICollection<GtaplayerVehicles> GtaplayerVehicles { get; set; }
    }
}