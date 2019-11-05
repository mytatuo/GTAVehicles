using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace GTAVehicles.Models
{
    public partial class GTAContext : DbContext
    {
        public virtual DbSet<GTAVehiclesRanked> GTAVehiclesRanked { get; set; }
    }

    public partial class GTAVehiclesRanked
    {
        public int Id { get; set; }
        public string VehicleModel { get; set; }
        public int ClassId { get; set; }
        public string ClassName { get; set; }
        public string TrackSpeed { get; set; }
        public int TrackRank { get; set; }
        public int TrackRankInClass { get; set; }
        public decimal DragSpeed { get; set; }
        public int DragRank { get; set; }
        public int DragRankInClass { get; set; }
    }

    public partial class GtavehicleDropdownClass
    {
        public string Id { get; set; }
        public string ClassName { get; set; }
    }

}
