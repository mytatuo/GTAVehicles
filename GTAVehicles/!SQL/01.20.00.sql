/****** Object:  StoredProcedure [dbo].[sp_GTAVehiclesRankedByUser]    Script Date: 4/11/2020 4:27:35 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_GTAVehiclesRankedByUser]
@UserName nvarchar(500)
AS
BEGIN
	SET NOCOUNT ON;
SELECT DISTINCT        
dbo.GTAVehicles.ID,
dbo.GTAVehicles.VehicleModel,
dbo.GTAVehicles.ClassID,
dbo.GTAVehicleClass.ClassName, 
dbo.GTAVehicles.TrackSpeed,
CAST(RANK() OVER (ORDER BY dbo.GTAVehicles.TrackSpeed ASC) AS INT) AS TrackRank,
CAST(RANK() OVER (PARTITION BY dbo.GTAVehicles.ClassID ORDER BY dbo.GTAVehicles.TrackSpeed ASC) AS INT) AS TrackRankInClass,
dbo.GTAVehicles.DragSpeed,
CAST(RANK() OVER (ORDER BY dbo.GTAVehicles.DragSpeed DESC) AS INT) AS DragRank,
CAST(RANK() OVER (PARTITION BY dbo.GTAVehicles.ClassID ORDER BY dbo.GTAVehicles.DragSpeed DESC) AS INT) AS DragRankInClass,
GTAPlayers.UserName,

STUFF((SELECT ';' + 
CONVERT(nvarchar(10),GTAPlayerCharacters.CharacterColor) + ':' + GTAPlayerCharacters.CharacterName
FROM            GTAVehicleClass INNER JOIN
                         GTAVehicles GTAVehiclesSubQuery ON GTAVehicleClass.ID = GTAVehiclesSubQuery.ClassID LEFT OUTER JOIN
                         GTAPlayerVehicles ON GTAVehiclesSubQuery.ID = GTAPlayerVehicles.VehicleID LEFT OUTER JOIN
                         GTAPlayerGarages LEFT OUTER JOIN
                         GTAPlayerCharacters ON GTAPlayerGarages.CharacterID = GTAPlayerCharacters.ID LEFT OUTER JOIN
                         GTAPlayers GTAPlayersSubQuery ON GTAPlayerCharacters.PlayerID = GTAPlayersSubQuery.ID ON GTAPlayerVehicles.PlayerGarageID = GTAPlayerGarages.ID
WHERE        (GTAPlayersSubQuery.UserName = GTAPlayers.UserName) AND (GTAVehiclesSubQuery.ID = dbo.GTAVehicles.ID)

          FOR XML PATH('')), 1, 1, '') CharactersOwningVehicle


FROM            GTAVehicleClass INNER JOIN
                         GTAVehicles ON GTAVehicleClass.ID = GTAVehicles.ClassID LEFT OUTER JOIN
                         GTAPlayerVehicles ON GTAVehicles.ID = GTAPlayerVehicles.VehicleID LEFT OUTER JOIN
                         GTAPlayerGarages LEFT OUTER JOIN
                         GTAPlayerCharacters ON GTAPlayerGarages.CharacterID = GTAPlayerCharacters.ID LEFT OUTER JOIN
                         GTAPlayers ON GTAPlayerCharacters.PlayerID = GTAPlayers.ID ON GTAPlayerVehicles.PlayerGarageID = GTAPlayerGarages.ID
WHERE        (GTAPlayers.UserName = @UserName) or (GTAPlayers.UserName is null) 
END
GO


