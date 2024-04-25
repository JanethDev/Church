USE [alexisnoches_SampleDB]
GO
/****** Object:  StoredProcedure [dbo].[stp_eft_movil]    Script Date: 3/20/2024 10:17:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*****************************************************************************/

ALTER PROCEDURE [dbo].[stp_access]	
(
	@method 					VARCHAR(100)
	, @clientId					INT				= NULL
	, @cardId					INT				= NULL
	, @card						VARCHAR(500)	= NULL
	, @nip						VARCHAR(500)	= NULL
)
 
AS
SET NOCOUNT ON
-----------------------------------------------------------------------------------------------------------
IF @method = 'createUserEmployee' BEGIN
	select ''
END
-----------------------------------------------------------------------------------------------------------