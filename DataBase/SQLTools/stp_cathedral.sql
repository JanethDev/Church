USE [alexisnoches_SampleDB]
GO
/****** Object:  StoredProcedure [dbo].[stp_roles]    Script Date: 4/29/2024 12:03:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*****************************************************************************/

ALTER PROCEDURE [dbo].[stp_cathedral]	
(
	@method 					VARCHAR(100)
	, @name						VARCHAR(100) = NULL
	, @cityId					INT			 = NULL	
	, @logMessageNew			VARCHAR(MAX) = NULL
	, @userId					INT			 = NULL
)
 
AS
SET NOCOUNT ON
-----------------------------------------------------------------------------------------------------------
IF @method = 'consultAll' BEGIN
	SELECT 
	ca.*,
	ci.city 
	FROM cat_cathedral ca
	INNER JOIN cat_cities ci ON ca.cat_cities_id = ci.id 
END
-----------------------------------------------------------------------------------------------------------
IF @method = 'create' BEGIN

	BEGIN TRANSACTION;
	
	INSERT INTO cat_cathedral([name],[cat_cities_id])
	VALUES (@name,@cityId)

	IF @@ERROR <> 0
	BEGIN
		-- Si hay un error, realiza un rollback para deshacer los cambios
		ROLLBACK TRANSACTION;
		SELECT 
		-1 as 'code',
		'Eror al momento de insertar la catedral' as 'message'
		RETURN;
	END

	COMMIT TRANSACTION;

	-- Guardar registro
	SET @logMessageNew = CONCAT('Creación de catedral: ', @name)
	INSERT INTO log_changes(new_value,cat_user_id,log_date)
	VALUES (@logMessageNew,@userId,GETDATE())

	SELECT 
	1 as 'code',
	'catedral creada con éxito' as 'message'
END
-----------------------------------------------------------------------------------------------------------
