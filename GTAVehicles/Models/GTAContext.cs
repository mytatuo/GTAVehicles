using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace GTAVehicles.Models
{
    public partial class GTAContext : DbContext
    {
        public GTAContext()
        {
        }

        public GTAContext(DbContextOptions<GTAContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Gtagarages> Gtagarages { get; set; }
        public virtual DbSet<GtaplayerCharacters> GtaplayerCharacters { get; set; }
        public virtual DbSet<GtaplayerGarages> GtaplayerGarages { get; set; }
        public virtual DbSet<GtaplayerVehicleMods> GtaplayerVehicleMods { get; set; }
        public virtual DbSet<GtaplayerVehicles> GtaplayerVehicles { get; set; }
        public virtual DbSet<Gtaplayers> Gtaplayers { get; set; }
        public virtual DbSet<GtavehicleClass> GtavehicleClass { get; set; }
        public virtual DbSet<GtavehicleModTypes> GtavehicleModTypes { get; set; }
        public virtual DbSet<Gtavehicles> Gtavehicles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.0-rtm-35687");

            modelBuilder.Entity<Gtagarages>(entity =>
            {
                entity.ToTable("GTAGarages");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.GarageName).HasMaxLength(255);
            });

            modelBuilder.Entity<GtaplayerCharacters>(entity =>
            {
                entity.ToTable("GTAPlayerCharacters");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CharacterName).HasMaxLength(255);
            });

            modelBuilder.Entity<GtaplayerGarages>(entity =>
            {
                entity.ToTable("GTAPlayerGarages");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CharacterId).HasColumnName("CharacterID");

                entity.HasOne(d => d.Character)
                    .WithMany(p => p.GtaplayerGarages)
                    .HasForeignKey(d => d.CharacterId)
                    .HasConstraintName("FK_GTAGarages_GTACharacters");

                entity.HasOne(d => d.Garage)
                    .WithMany(p => p.GtaplayerGarages)
                    .HasForeignKey(d => d.GarageId)
                    .HasConstraintName("FK_GTAPlayerGarages_GTAGarages");
            });

            modelBuilder.Entity<GtaplayerVehicleMods>(entity =>
            {
                entity.ToTable("GTAPlayerVehicleMods");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ModTypeId).HasColumnName("ModTypeID");

                entity.Property(e => e.ModVariation).HasMaxLength(255);

                entity.Property(e => e.VehicleOwnedId).HasColumnName("VehicleOwnedID");

                entity.HasOne(d => d.ModType)
                    .WithMany(p => p.GtaplayerVehicleMods)
                    .HasForeignKey(d => d.ModTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GTAVehicleMods_GTAVehicleModTypes");

                entity.HasOne(d => d.VehicleOwned)
                    .WithMany(p => p.GtaplayerVehicleMods)
                    .HasForeignKey(d => d.VehicleOwnedId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GTAVehicleMods_GTAVehiclesOwned");
            });

            modelBuilder.Entity<GtaplayerVehicles>(entity =>
            {
                entity.ToTable("GTAPlayerVehicles");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.PlayerGarageId).HasColumnName("PlayerGarageID");

                entity.Property(e => e.PlayerId).HasColumnName("PlayerID");

                entity.Property(e => e.VehicleId).HasColumnName("VehicleID");

                entity.HasOne(d => d.PlayerGarage)
                    .WithMany(p => p.GtaplayerVehicles)
                    .HasForeignKey(d => d.PlayerGarageId)
                    .HasConstraintName("FK_GTAVehiclesOwned_GTAGarages");

                entity.HasOne(d => d.Player)
                    .WithMany(p => p.GtaplayerVehicles)
                    .HasForeignKey(d => d.PlayerId)
                    .HasConstraintName("FK_GTAPlayerVehicles_GTAPlayers");

                entity.HasOne(d => d.Vehicle)
                    .WithMany(p => p.GtaplayerVehicles)
                    .HasForeignKey(d => d.VehicleId)
                    .HasConstraintName("FK_GTAPlayerVehicles_GTAVehicles");
            });

            modelBuilder.Entity<Gtaplayers>(entity =>
            {
                entity.ToTable("GTAPlayers");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<GtavehicleClass>(entity =>
            {
                entity.ToTable("GTAVehicleClass");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ClassName).HasMaxLength(255);
            });

            modelBuilder.Entity<GtavehicleModTypes>(entity =>
            {
                entity.ToTable("GTAVehicleModTypes");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ModType).HasMaxLength(255);
            });

            modelBuilder.Entity<Gtavehicles>(entity =>
            {
                entity.ToTable("GTAVehicles");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ClassId).HasColumnName("ClassID");

                entity.Property(e => e.DragSpeed).HasColumnType("money");

                entity.Property(e => e.TrackSpeed).HasMaxLength(255);

                entity.Property(e => e.VehicleModel).HasMaxLength(255);

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.Gtavehicles)
                    .HasForeignKey(d => d.ClassId)
                    .HasConstraintName("FK_GTAVehicles_GTAVehicleClass");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}