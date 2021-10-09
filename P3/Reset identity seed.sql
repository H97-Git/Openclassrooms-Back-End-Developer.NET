USE [P3Referential-2f561d3b-493f-46fd-83c9-6e2643e7bd0a]
GO

DELETE FROM [dbo].[Product]
DBCC CHECKIDENT ('[dbo].[Product]', RESEED, 0);
GO


