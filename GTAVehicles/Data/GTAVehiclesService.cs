using GTAVehicles.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System;

namespace GTAVehicles.Data
{
    public class GTAVehiclesService
    {
        private readonly GTAContext _context;
        public GTAVehiclesService(GTAContext context)
        {
            _context = context;
        }

        public Task<Gtaplayers> GetPlayerAsync(string strCurrentUser)
        {
            Gtaplayers colGTAPlayer = new Gtaplayers();
            // Get player's DB entry

            colGTAPlayer = (from Gtaplayers in _context.Gtaplayers
                                // only get entries for the current logged-in user
                            where Gtaplayers.UserName == strCurrentUser
                            select Gtaplayers).AsNoTracking().FirstOrDefault();

            return Task.FromResult(colGTAPlayer);
        }

        public Task<bool> CreatePlayerAsync(Gtaplayers objGTAPlayer)
        {
            _context.Gtaplayers.Add(objGTAPlayer);
            _context.SaveChanges();

            return Task.FromResult(true);
        }

        public Task<List<GtavehicleClass>> GetGTAVehicleClassesAsync()
        {
            List<GtavehicleClass> colGTAVehicleClasses = new List<GtavehicleClass>();

            // get all vehicle classes in GTA
            colGTAVehicleClasses = (from gtaclasses in _context.GtavehicleClass
                                    select gtaclasses).AsNoTracking().ToList();

            return Task.FromResult(colGTAVehicleClasses);
        }

        public Task<IQueryable<GTAVehiclesOwned>> GetGTAVehiclesOwnedAsync(int GarageId)
        {
            var result = _context.GTAVehiclesOwned
                .FromSqlRaw("SELECT * FROM [Manipulyte].[dbo].[vw_GTAVehiclesOwned]").Where(x => x.PlayerGarageID == GarageId)
                .Select(x => new GTAVehiclesOwned
                {
                    Id = x.Id,
                    VehicleModel = x.VehicleModel,
                    CharacterName = x.CharacterName,
                    CharacterID = x.CharacterID,
                    GarageName = x.GarageName,
                    PlayerGarageID = x.PlayerGarageID,
                    VehicleID = x.VehicleID
                }).AsNoTracking().AsQueryable();

            return Task.FromResult(result);
        }

        public Task<IQueryable<GTAVehiclesRanked>> GetGTAVehiclesRankedAsync(List<int> classIDs)
        {
            // get all vehicles in GTA
            if (classIDs.FirstOrDefault() == 0)
            {
                var result = _context.GTAVehiclesRanked
                              .FromSqlRaw("SELECT * FROM [Manipulyte].[dbo].[vw_GTAVehiclesRanked]")
                              .Select(x => new GTAVehiclesRanked
                              {
                                  ClassId = x.ClassId,
                                  ClassName = x.ClassName,
                                  DragRank = x.DragRank,
                                  DragRankInClass = x.DragRankInClass,
                                  DragSpeed = x.DragSpeed,
                                  Id = x.Id,
                                  TrackRank = x.TrackRank,
                                  TrackRankInClass = x.TrackRankInClass,
                                  TrackSpeed = x.TrackSpeed,
                                  VehicleModel = x.VehicleModel
                              }).AsNoTracking();

                return Task.FromResult(result);
            }
            else
            {
                var result = _context.GTAVehiclesRanked
                    .FromSqlRaw("SELECT * FROM [Manipulyte].[dbo].[vw_GTAVehiclesRanked]").Where(x => classIDs.Contains(x.ClassId))
                    .Select(x => new GTAVehiclesRanked
                    {
                        ClassId = x.ClassId,
                        ClassName = x.ClassName,
                        DragRank = x.DragRank,
                        DragRankInClass = x.DragRankInClass,
                        DragSpeed = x.DragSpeed,
                        Id = x.Id,
                        TrackRank = x.TrackRank,
                        TrackRankInClass = x.TrackRankInClass,
                        TrackSpeed = x.TrackSpeed,
                        VehicleModel = x.VehicleModel
                    }).AsNoTracking().AsQueryable();

                return Task.FromResult(result);
            }
        }

        public Task<List<GtaplayerCharacters>> GetCharactersAsync(Gtaplayers player)
        {
            List<GtaplayerCharacters> colGTACharacters = new List<GtaplayerCharacters>();
            // get GTA Player's Characters

            // only get character(s) for the current logged-in user
            colGTACharacters = (from gtacharacter in _context.GtaplayerCharacters
                                .Include(GtaplayerGarage => GtaplayerGarage.GtaplayerGarages)
                                where gtacharacter.PlayerId == player.Id
                                select gtacharacter).AsNoTracking().ToList();

            return Task.FromResult(colGTACharacters);
        }

        public Task<List<GtacharactersDTO>> GetCharactersDTOAsync(Gtaplayers player)
        {
            List<GtacharactersDTO> colGTACharacters = new List<GtacharactersDTO>();
            // get GTA Player's Characters

            // only get character(s) for the current logged-in user
            colGTACharacters = (from gtacharacter in _context.GtaplayerCharacters
                                .Include(GtaplayerGarage => GtaplayerGarage.GtaplayerGarages)
                                where gtacharacter.PlayerId == player.Id
                                select new GtacharactersDTO
                                {
                                    Id = gtacharacter.Id.ToString(),
                                    UserName = gtacharacter.CharacterName,
                                    GtaplayerGarages = gtacharacter.GtaplayerGarages.ToList()
                                }).AsNoTracking().ToList();

            return Task.FromResult(colGTACharacters);
        }

        public Task<GtaplayerCharacters> CreateCharacterAsync(GtaplayerCharacters objGtaplayerCharacter)
        {
            _context.GtaplayerCharacters.Add(objGtaplayerCharacter);
            _context.SaveChanges();

            return Task.FromResult(objGtaplayerCharacter);
        }

        public Task<bool> UpdateCharacterAsync(GtaplayerCharacters objGtaplayerCharacter)
        {
            var ExistingCharacter =
                _context.GtaplayerCharacters
                .Where(x => x.Id == objGtaplayerCharacter.Id)
                .FirstOrDefault();

            if (ExistingCharacter != null)
            {
                ExistingCharacter.CharacterName =
                    objGtaplayerCharacter.CharacterName;
                ExistingCharacter.CharacterColor =
                    objGtaplayerCharacter.CharacterColor;

                _context.SaveChanges();
            }
            else
            {
                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }

        public Task<bool> DeleteCharacterAsync(GtaplayerCharacters objGtaplayerCharacter)
        {

            var ExistingCharacter =
                _context.GtaplayerCharacters
                .Where(x => x.Id == objGtaplayerCharacter.Id)
                .FirstOrDefault();

            if (ExistingCharacter != null)
            {
                _context.GtaplayerCharacters.Remove(ExistingCharacter);
                _context.SaveChanges();
            }
            else
            {
                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }

        public Task<List<GtaplayerGarages>> GetGaragesAsync(GtaplayerCharacters character)
        {
            List<GtaplayerGarages> colGTAGarages = new List<GtaplayerGarages>();
            // get GTA Garages under a particular Character

            colGTAGarages = (from gtagarage in _context.GtaplayerGarages
                             where gtagarage.CharacterId == character.Id
                             select gtagarage).AsNoTracking().ToList();

            return Task.FromResult(colGTAGarages);
        }

        public Task<List<GtaplayerGaragesDTO>> GetGaragesForPlayerAsync(Gtaplayers player)
        {
            List<GtaplayerGaragesDTO> colGTAGarages = new List<GtaplayerGaragesDTO>();
            // get GTA Garages under a particular Player

            colGTAGarages = (from gtagarage in _context.GtaplayerGarages
                             where gtagarage.Character.Player.Id == player.Id
                             select new GtaplayerGaragesDTO
                             {
                                 Id = gtagarage.Id.ToString(),
                                 CharacterId = gtagarage.CharacterId.ToString(),
                                 GarageId = (gtagarage.GarageId != null) ? gtagarage.GarageId.Value.ToString() : "",
                                 GarageName = gtagarage.GarageName

                             }).AsNoTracking().ToList();

            return Task.FromResult(colGTAGarages);
        }

        public Task<GtaplayerGarages> CreateGarageAsync(GtaplayerGarages objGtaplayerGarage)
        {
            try
            {
                _context.GtaplayerGarages.Add(objGtaplayerGarage);
                _context.SaveChanges();
            }
            catch (System.Exception ex)
            {
                string myerror = ex.GetBaseException().ToString();
            }

            return Task.FromResult(objGtaplayerGarage);
        }

        public Task<bool> UpdateGarageAsync(GtaplayerGarages objGtaplayerGarage)
        {
            var ExistingGarage =
                _context.GtaplayerGarages
                .Where(x => x.Id == objGtaplayerGarage.Id)
                .FirstOrDefault();

            if (ExistingGarage != null)
            {
                ExistingGarage.GarageName =
                    objGtaplayerGarage.GarageName;

                _context.SaveChanges();
            }
            else
            {
                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }

        public Task<bool> DeleteGarageAsync(GtaplayerGarages objGtaplayerGarage)
        {
            var ExistingGarage =
                _context.GtaplayerGarages
                .Where(x => x.Id == objGtaplayerGarage.Id)
                .FirstOrDefault();

            if (ExistingGarage != null)
            {
                _context.GtaplayerGarages.Remove(ExistingGarage);
                _context.SaveChanges();
            }
            else
            {
                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }

        public Task<bool> TransferVehicleAsync(GtaplayerVehicles SelectedVehicle, GtaplayerGaragesDTO TransferGarage)
        {
            // get a reference to the actual database record
            var GtaplayerVehicle =
                _context.GtaplayerVehicles
               .Where(x => x.Id == SelectedVehicle.Id)
               .FirstOrDefault();

            if (GtaplayerVehicle != null)
            {
                GtaplayerVehicle.PlayerGarageId = Convert.ToInt32(TransferGarage.Id);

                _context.SaveChanges();
            }
            else
            {
                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }

        public Task<GtaplayerVehicles> GetGTAPlayerVehicleAsync(GTAVehiclesOwned VehicleOwned)
        {
            GtaplayerVehicles GTAPlayerVehicle = new GtaplayerVehicles();

            GTAPlayerVehicle = (from veh in _context.GtaplayerVehicles
                                where veh.Id == VehicleOwned.Id
                                select veh).AsNoTracking().FirstOrDefault();

            return Task.FromResult(GTAPlayerVehicle);
        }

        public Task<GtaplayerVehicles> StoreVehicleAsync(GtaplayerVehicles objGtaplayerVehicle)
        {
            _context.GtaplayerVehicles.Add(objGtaplayerVehicle);
            _context.SaveChanges();

            return Task.FromResult(objGtaplayerVehicle);
        }

        public Task<bool> RemoveVehicleAsync(GtaplayerVehicles objGtaplayerVehicle)
        {
            // get a reference to the actual database record
            var GtaplayerVehicle =
                _context.GtaplayerVehicles
               .Where(x => x.Id == objGtaplayerVehicle.Id)
               .FirstOrDefault();

            if (GtaplayerVehicle != null)
            {
                _context.GtaplayerVehicles.Remove(GtaplayerVehicle);
                _context.SaveChanges();
            }
            else
            {
                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }

    }
}
