CREATE PROCEDURE UPDATE_QUESTION
@ID INT,
@ORDER INT,
@TEXT NVARCHAR(4000)

AS
SET NOCOUNT ON

DECLARE @@MyOrder as INT
SET @@MyOrder = (SELECT [Order] FROM Questions WHERE ID = @ID)

BEGIN TRY
	IF ((SELECT COUNT(ID) FROM Questions WHERE [Order] = @ORDER) = 0)
		BEGIN
			UPDATE Questions
			SET [Text] = @TEXT
			WHERE [ID] = @ID
			RETURN 1
		END
	ELSE
		BEGIN
			IF (@@MyOrder = @ORDER)
				BEGIN
					UPDATE Questions
					SET [Order] = @ORDER, [Text] = @TEXT
					WHERE [ID] = @ID
					RETURN 1
				END
		END

		RETURN 2 --ORDER ALREADY TAKEN

END TRY
BEGIN CATCH
		RETURN 3 --ERROR
END CATCH