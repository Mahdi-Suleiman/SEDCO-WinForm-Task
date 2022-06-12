CREATE PROCEDURE UPDATE_SMILEY_QUESTION
@ID INT,
@Order INT,
@Text NVARCHAR(4000),
@Type INT,
@NumberOfSmileyFaces INT
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

	UPDATE Smiley_Questions
	SET NumberOfSmileyFaces = @NumberOfSmileyFaces
	WHERE ID = @ID

	SELECT @SUCCESS AS ErrorCode, ERROR_MESSAGE() AS ErrorMessage
	RETURN @SUCCESS

END TRY
BEGIN CATCH
	SELECT @ERROR AS ErrorCode, ERROR_MESSAGE() AS ErrorMessage
	RETURN @ERROR
END CATCH