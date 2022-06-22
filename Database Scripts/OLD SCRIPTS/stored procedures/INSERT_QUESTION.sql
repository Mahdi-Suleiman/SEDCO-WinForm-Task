ALTER PROCEDURE INSERT_SMILEY_QUESTION
@ORDER INT,
@TEXT NVARCHAR(4000),
@TYPE INT,
@NumberOfSmileyFaces INT
AS
SET NOCOUNT ON

BEGIN TRY
	DECLARE @SUCCESS INT, @ERROR INT
	SET @SUCCESS = 1
	SET @ERROR = -1

    INSERT INTO Questions 
    ([Order], [Text], [Type])
    VALUES 
    (@ORDER, @TEXT, @TYPE)
	
	IF(SCOPE_IDENTITY() IS NOT NULL)
	BEGIN
	INSERT INTO Smiley_Questions(ID, NumberOfSmileyFaces)
	VALUES (SCOPE_IDENTITY(), @NumberOfSmileyFaces)
	
	SELECT @SUCCESS AS ErrorCode, ERROR_MESSAGE() AS ErrorMessage
	RETURN @SUCCESS
	END
END TRY
BEGIN CATCH
	SELECT @ERROR AS ErrorCode, ERROR_MESSAGE() AS ErrorMessage
	RETURN @ERROR
END CATCH