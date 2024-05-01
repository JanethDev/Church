USE [alexisnoches_SampleDB]
GO
/****** Object:  StoredProcedure [dbo].[stp_access]    Script Date: 4/25/2024 8:42:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*****************************************************************************/

ALTER PROCEDURE [dbo].[stp_roles]	
(
	@method 					VARCHAR(100)
)
 
AS
SET NOCOUNT ON
-----------------------------------------------------------------------------------------------------------
IF @method = 'consultAll' BEGIN
	SELECT * FROM cat_roles WHERE id != '7'
END
-----------------------------------------------------------------------------------------------------------