using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace GTAVehicles.Models
{
    public partial class GTAContext : DbContext
    {
        public virtual DbSet<GTAVehiclesRanked> GTAVehiclesRanked { get; set; }
        public virtual DbSet<GTAVehiclesOwned> GTAVehiclesOwned { get; set; }
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

    public partial class GTAVehiclesOwned
    {
        public int Id { get; set; }
        public string VehicleModel { get; set; }
        public string CharacterName { get; set; }
        public int CharacterID { get; set; }
        public string GarageName { get; set; }
        public int PlayerGarageID { get; set; }
        public int VehicleID { get; set; }
    }

    public partial class GtavehicleDropdownClass
    {
        public string Id { get; set; }
        public string ClassName { get; set; }
    }

    public class GtavehicleColors
    {
        public string ColorName { get; set; }
        public string BackgroundColor { get; set; }
        public string ForegroundColor { get; set; }
    }
}
