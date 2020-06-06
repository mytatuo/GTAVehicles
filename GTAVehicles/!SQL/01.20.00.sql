/****** Object:  StoredProcedure [dbo].[sp_GTAVehiclesRankedByUser]    Script Date: 6/6/2020 10:24:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_GTAVehiclesRankedByUser]
@UserName nvarchar(500)
AS
BEGIN
	SET NOCOUNT ON;

    DECLARE @VehiclesRanked table
	(
	ID int NOT NULL, 
	VehicleModel nvarchar(500) NOT NULL, 
	ClassID int NOT NULL, 
	ClassName nvarchar(500) NOT NULL,
	TrackSpeed [nvarchar](255) NULL, 
	TrackRank int NOT NULL, 
    TrackRankInClass int NOT NULL,
    DragSpeed [money] NULL, 
	DragRank int NOT NULL, 
    DragRankInClass int NOT NULL
	PRIMARY KEY (ID) -- This key helps performance or UNIQUE CLUSTERED as used below
	--UNIQUE CLUSTERED (TransactionID, ProductID, TransactionDate)
	)

	Insert Into @VehiclesRanked
	(
	ID,
	VehicleModel, 
	ClassID, 
	ClassName,
	TrackSpeed, 
	TrackRank, 
    TrackRankInClass,
    DragSpeed, 
	DragRank, 
    DragRankInClass
	)
    SELECT        
	dbo.GTAVehicles.ID, dbo.GTAVehicles.VehicleModel, dbo.GTAVehicles.ClassID, dbo.GTAVehicleClass.ClassName, dbo.GTAVehicles.TrackSpeed, 
	CAST(RANK() OVER (ORDER BY dbo.GTAVehicles.TrackSpeed ASC) AS INT) AS TrackRank, 
	CAST(RANK() OVER (PARTITION BY dbo.GTAVehicles.ClassID ORDER BY dbo.GTAVehicles.TrackSpeed ASC) AS INT) AS TrackRankInClass, 
	dbo.GTAVehicles.DragSpeed, 
	CAST(RANK() OVER (ORDER BY dbo.GTAVehicles.DragSpeed DESC) AS INT) AS DragRank, 
	CAST(RANK() OVER (PARTITION BY dbo.GTAVehicles.ClassID ORDER BY dbo.GTAVehicles.DragSpeed DESC) AS INT) AS DragRankInClass
	FROM            
	dbo.GTAVehicleClass INNER JOIN dbo.GTAVehicles ON dbo.GTAVehicleClass.ID = dbo.GTAVehicles.ClassID

SELECT DISTINCT        
dbo.GTAVehicles.ID,
dbo.GTAVehicles.VehicleModel,
dbo.GTAVehicles.ClassID,
dbo.GTAVehicleClass.ClassName, 
dbo.GTAVehicles.TrackSpeed,
(select TrackRank from @VehiclesRanked where ID = dbo.GTAVehicles.ID) AS TrackRank,
(select TrackRankInClass from @VehiclesRanked where ID = dbo.GTAVehicles.ID) AS TrackRankInClass,
dbo.GTAVehicles.DragSpeed,
(select DragRank from @VehiclesRanked where ID = dbo.GTAVehicles.ID) AS DragRank,
(select DragRankInClass from @VehiclesRanked where ID = dbo.GTAVehicles.ID) AS DragRankInClass,
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