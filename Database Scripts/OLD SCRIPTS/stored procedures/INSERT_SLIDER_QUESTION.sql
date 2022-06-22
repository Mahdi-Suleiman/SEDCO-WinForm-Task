ALTER PROCEDURE INSERT_SLIDER_QUESTION
@Order INT,
@Text NVARCHAR(4000),
@Type INT,

AS
SET NOCOUNT ON

BEGIN TRY
	DECLARE @SUCCESS INT, @ERROR INT
	SET @SUCCESS = 1
	SET @ERROR = -1

    INSERT INTO Questions 
    ([Order], [Text], [Type])
    VALUES 
    (@Order, @Text, @Type)
	
	INSERT INTO Slider_Questions
	(ID, StartValue, EndValue, StartValueCaption, EndValueCaption)
	VALUES (SCOPE_IDENTITY(), @StartValue, @EndValue, @StartValueCaption, @EndValueCaption)
	
	SELECT @SUCCESS AS ErrorCode, ERROR_MESSAGE() AS ErrorMessage
	RETURN @SUCCESS

END TRY
BEGIN CATCH
	SELECT @ERROR AS ErrorCode, ERROR_MESSAGE() AS ErrorMessage
	RETURN @ERROR
END CATCH