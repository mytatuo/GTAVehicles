using System;
using System.Collections.Generic;

namespace GTAVehicles.Models
{
    public partial class Gtagarages
    {
        public Gtagarages()
        {
            GtaplayerGarages = new HashSet<GtaplayerGarages>();
        }

        public int Id { get; set; }
        public string GarageName { get; set; }

        public virtual ICollection<GtaplayerGarages> GtaplayerGarages { get; set; }
    }
}