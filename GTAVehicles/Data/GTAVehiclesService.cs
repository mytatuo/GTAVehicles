using GTAVehicles.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

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
                            select Gtaplayers).FirstOrDefault();

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
                                    select gtaclasses).ToList();

            return Task.FromResult(colGTAVehicleClasses);
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
                              });

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
                    }).AsQueryable();

                return Task.FromResult(result);
            }
        }

        public Task<List<GtaplayerCharacters>> GetCharactersAsync(Gtaplayers player)
        {
            List<GtaplayerCharacters> colGTACharacters = new List<GtaplayerCharacters>();
            // get GTA Player's Characters

            colGTACharacters = (from gtacharacter in _context.GtaplayerCharacters
                                    // only get character(s) for the current logged-in user
                                where gtacharacter.PlayerID == player.Id
                                select gtacharacter).ToList();

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
    }
}
