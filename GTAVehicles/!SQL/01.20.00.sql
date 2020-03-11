SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[sp_GTAVehiclesRankedByUser]
@UserName nvarchar(500)
AS
BEGIN
	SET NOCOUNT ON;
SELECT        dbo.GTAVehicles.ID,
dbo.GTAVehicles.VehicleModel,
dbo.GTAVehicles.ClassID,
dbo.GTAVehicleClass.ClassName, 
dbo.GTAVehicles.TrackSpeed,
CAST(RANK() OVER (ORDER BY dbo.GTAVehicles.TrackSpeed ASC) AS INT) AS TrackRank,
CAST(RANK() OVER (PARTITION BY dbo.GTAVehicles.ClassID ORDER BY dbo.GTAVehicles.TrackSpeed ASC) AS INT) AS TrackRankInClass,
dbo.GTAVehicles.DragSpeed,
CAST(RANK() OVER (ORDER BY dbo.GTAVehicles.DragSpeed DESC) AS INT) AS DragRank,
CAST(RANK() OVER (PARTITION BY dbo.GTAVehicles.ClassID ORDER BY dbo.GTAVehicles.DragSpeed DESC) AS INT) AS DragRankInClass
FROM            dbo.GTAVehicleClass INNER JOIN
                         dbo.GTAVehicles ON dbo.GTAVehicleClass.ID = dbo.GTAVehicles.ClassID
END
GO
