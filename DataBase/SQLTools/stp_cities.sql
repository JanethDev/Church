USE [alexisnoches_SampleDB]
GO
/****** Object:  StoredProcedure [dbo].[stp_roles]    Script Date: 4/30/2024 9:13:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*****************************************************************************/

ALTER PROCEDURE [dbo].[stp_cities]	
(
	@method 					VARCHAR(100)
	, @city						VARCHAR(200) = NULL
	, @logMessageNew			VARCHAR(MAX) = NULL
	, @userId					INT			 = NULL
)
 
AS
SET NOCOUNT ON
-----------------------------------------------------------------------------------------------------------
IF @method = 'consultAll' BEGIN
	SELECT * FROM [cat_cities]
END
-----------------------------------------------------------------------------------------------------------
IF @method = 'create' BEGIN
	BEGIN TRANSACTION;
	
	INSERT INTO [cat_cities] (city)
	VALUES (@city)

	IF @@ERROR <> 0
	BEGIN
		-- Si hay un error, realiza un rollback para deshacer los cambios
		ROLLBACK TRANSACTION;
		SELECT 
		-1 as 'code',
		'Eror al momento de insertar ciudad' as 'message'
		RETURN;
	END

	COMMIT TRANSACTION;

	-- Guardar registro
	SET @logMessageNew = CONCAT('Creación de ciudad: ', @city)
	INSERT INTO log_changes(new_value,cat_user_id,log_date)
	VALUES (@logMessageNew,@userId,GETDATE())

	SELECT 
	1 as 'code',
	'ciudad creada con éxito' as 'message'
END
-----------------------------------------------------------------------------------------------------------