USE [alexisnoches_SampleDB]
GO
/****** Object:  StoredProcedure [dbo].[stp_roles]    Script Date: 4/30/2024 9:13:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*****************************************************************************/

ALTER PROCEDURE [dbo].[stp_days]
(
	@method 					VARCHAR(100)
)
 
AS
SET NOCOUNT ON
-----------------------------------------------------------------------------------------------------------
IF @method = 'consultAll' BEGIN
	SELECT * FROM [cat_days]
END
-----------------------------------------------------------------------------------------------------------