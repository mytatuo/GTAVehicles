using System;
using System.Collections.Generic;

namespace GTAVehicles.Models
{
    public partial class Gtavehicles
    {
        public Gtavehicles()
        {
            GtaplayerVehicles = new HashSet<GtaplayerVehicles>();
        }

        public int Id { get; set; }
        public string VehicleModel { get; set; }
        public int? ClassId { get; set; }
        public string TrackSpeed { get; set; }
        public decimal? DragSpeed { get; set; }

        public virtual GtavehicleClass Class { get; set; }
        public virtual ICollection<GtaplayerVehicles> GtaplayerVehicles { get; set; }
    }
}