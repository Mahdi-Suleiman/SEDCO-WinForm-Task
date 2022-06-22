CREATE PROCEDURE UPDATE_SLIDER_QUESTION
@ID INT,
@Order INT,
@Text NVARCHAR(4000),
@Type INT,
@StartValue INT,
@EndValue INT,
@StartValueCaption nvarchar(100),
@EndValueCaption nvarchar(100)

AS
SET NOCOUNT ON

--DECLARE @@MyOrder as INT
--SET @@MyOrder = (SELECT [Order] FROM Questions WHERE ID = @ID)

BEGIN TRY
	DECLARE @SUCCESS INT, @ERROR INT
	SET @SUCCESS = 1
	SET @ERROR = -1

	UPDATE Questions
	SET [Order] = @Order, [Text] = @Text
	WHERE [ID] = @ID

	UPDATE Slider_Questions
    SET StartValue = @StartValue, EndValue = @EndValue, StartValueCaption = @StartValueCaption, EndValueCaption = @EndValueCaption
	WHERE ID = @ID

	SELECT @SUCCESS AS ErrorCode, ERROR_MESSAGE() AS ErrorMessage
	RETURN @SUCCESS

END TRY
BEGIN CATCH
	SELECT @ERROR AS ErrorCode, ERROR_MESSAGE() AS ErrorMessage
	RETURN @ERROR
END CATCH