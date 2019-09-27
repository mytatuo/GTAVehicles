using System;
using System.Collections.Generic;

namespace GTAVehicles.Models
{
    public partial class GtaplayerCharacters
    {
        public GtaplayerCharacters()
        {
            GtaplayerGarages = new HashSet<GtaplayerGarages>();
        }

        public int Id { get; set; }
        public string CharacterName { get; set; }
        public string CharacterColor { get; set; }
        public int PlayerID { get; set; }

        public virtual ICollection<GtaplayerGarages> GtaplayerGarages { get; set; }
    }
}