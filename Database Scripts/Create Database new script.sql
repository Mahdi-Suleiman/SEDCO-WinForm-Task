USE [master]
GO
/****** Object:  Database [SurveyQuestionsConfigurator]    Script Date: 12/06/2022 12:20:36 ******/
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'SurveyQuestionsConfigurator')
BEGIN
CREATE DATABASE [SurveyQuestionsConfigurator]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'SurveyQuestionsConfigurator', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\SurveyQuestionsConfigurator.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'SurveyQuestionsConfigurator_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\SurveyQuestionsConfigurator_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
END
GO
ALTER DATABASE [SurveyQuestionsConfigurator] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [SurveyQuestionsConfigurator].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [SurveyQuestionsConfigurator] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [SurveyQuestionsConfigurator] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [SurveyQuestionsConfigurator] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [SurveyQuestionsConfigurator] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [SurveyQuestionsConfigurator] SET ARITHABORT OFF 
GO
ALTER DATABASE [SurveyQuestionsConfigurator] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [SurveyQuestionsConfigurator] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [SurveyQuestionsConfigurator] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [SurveyQuestionsConfigurator] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [SurveyQuestionsConfigurator] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [SurveyQuestionsConfigurator] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [SurveyQuestionsConfigurator] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [SurveyQuestionsConfigurator] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [SurveyQuestionsConfigurator] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [SurveyQuestionsConfigurator] SET  ENABLE_BROKER 
GO
ALTER DATABASE [SurveyQuestionsConfigurator] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [SurveyQuestionsConfigurator] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [SurveyQuestionsConfigurator] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [SurveyQuestionsConfigurator] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [SurveyQuestionsConfigurator] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [SurveyQuestionsConfigurator] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [SurveyQuestionsConfigurator] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [SurveyQuestionsConfigurator] SET RECOVERY FULL 
GO
ALTER DATABASE [SurveyQuestionsConfigurator] SET  MULTI_USER 
GO
ALTER DATABASE [SurveyQuestionsConfigurator] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [SurveyQuestionsConfigurator] SET DB_CHAINING OFF 
GO
ALTER DATABASE [SurveyQuestionsConfigurator] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [SurveyQuestionsConfigurator] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [SurveyQuestionsConfigurator] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [SurveyQuestionsConfigurator] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'SurveyQuestionsConfigurator', N'ON'
GO
ALTER DATABASE [SurveyQuestionsConfigurator] SET QUERY_STORE = OFF
GO
USE [SurveyQuestionsConfigurator]
GO
/****** Object:  UserDefinedFunction [dbo].[CheckIfOrderExist]    Script Date: 12/06/2022 12:20:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CheckIfOrderExist]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[CheckIfOrderExist] (@ORDER INT)
RETURNS INT
AS
BEGIN
	DECLARE @SUCCESS INT, @SQL_VIOLATION INT, @ID INT
	SET @SUCCESS = 1
	SET @SQL_VIOLATION = 2


	IF((SELECT COUNT(ID) FROM Questions WHERE [Order] = @ORDER) = 0)
		RETURN @SUCCESS

	RETURN @SQL_VIOLATION
END;
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetIDFromOrder]    Script Date: 12/06/2022 12:20:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetIDFromOrder]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[GetIDFromOrder] (@ORDER INT)
RETURNS INT
AS
BEGIN
	DECLARE @ID INT
	SET @ID = (SELECT [ID] FROM Questions WHERE [Order] = @ORDER)
	IF(@ID IS NOT NULL)
		RETURN @ID

	RETURN 0
END;
' 
END
GO
/****** Object:  Table [dbo].[Questions]    Script Date: 12/06/2022 12:20:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Questions]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Questions](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Order] [int] NOT NULL,
	[Text] [nvarchar](4000) NOT NULL,
	[Type] [int] NOT NULL,
 CONSTRAINT [PK_Questions_1] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UniqueOrder_Questions] UNIQUE NONCLUSTERED 
(
	[Order] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[Slider_Questions]    Script Date: 12/06/2022 12:20:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Slider_Questions]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Slider_Questions](
	[ID] [int] NOT NULL,
	[StartValue] [int] NOT NULL,
	[EndValue] [int] NOT NULL,
	[StartValueCaption] [nvarchar](100) NOT NULL,
	[EndValueCaption] [nvarchar](100) NOT NULL,
 CONSTRAINT [IX_Slider_Questions] UNIQUE NONCLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[Smiley_Questions]    Script Date: 12/06/2022 12:20:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Smiley_Questions]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Smiley_Questions](
	[ID] [int] NOT NULL,
	[NumberOfSmileyFaces] [int] NOT NULL,
 CONSTRAINT [IX_Smiley_Questions] UNIQUE NONCLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[Star_Questions]    Script Date: 12/06/2022 12:20:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Star_Questions]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Star_Questions](
	[ID] [int] NOT NULL,
	[NumberOfStars] [int] NOT NULL,
 CONSTRAINT [IX_Star_Questions] UNIQUE NONCLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Slider_Questions_Questions]') AND parent_object_id = OBJECT_ID(N'[dbo].[Slider_Questions]'))
ALTER TABLE [dbo].[Slider_Questions]  WITH CHECK ADD  CONSTRAINT [FK_Slider_Questions_Questions] FOREIGN KEY([ID])
REFERENCES [dbo].[Questions] ([ID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Slider_Questions_Questions]') AND parent_object_id = OBJECT_ID(N'[dbo].[Slider_Questions]'))
ALTER TABLE [dbo].[Slider_Questions] CHECK CONSTRAINT [FK_Slider_Questions_Questions]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Smiley_Questions_Questions]') AND parent_object_id = OBJECT_ID(N'[dbo].[Smiley_Questions]'))
ALTER TABLE [dbo].[Smiley_Questions]  WITH CHECK ADD  CONSTRAINT [FK_Smiley_Questions_Questions] FOREIGN KEY([ID])
REFERENCES [dbo].[Questions] ([ID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Smiley_Questions_Questions]') AND parent_object_id = OBJECT_ID(N'[dbo].[Smiley_Questions]'))
ALTER TABLE [dbo].[Smiley_Questions] CHECK CONSTRAINT [FK_Smiley_Questions_Questions]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Star_Questions_Questions]') AND parent_object_id = OBJECT_ID(N'[dbo].[Star_Questions]'))
ALTER TABLE [dbo].[Star_Questions]  WITH CHECK ADD  CONSTRAINT [FK_Star_Questions_Questions] FOREIGN KEY([ID])
REFERENCES [dbo].[Questions] ([ID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Star_Questions_Questions]') AND parent_object_id = OBJECT_ID(N'[dbo].[Star_Questions]'))
ALTER TABLE [dbo].[Star_Questions] CHECK CONSTRAINT [FK_Star_Questions_Questions]
GO
IF NOT EXISTS (SELECT * FROM sys.check_constraints WHERE object_id = OBJECT_ID(N'[dbo].[CK_Questions]') AND parent_object_id = OBJECT_ID(N'[dbo].[Questions]'))
ALTER TABLE [dbo].[Questions]  WITH CHECK ADD  CONSTRAINT [CK_Questions] CHECK  ((len([Text])<=(8000) AND ([Type]=(0) OR [Type]=(1) OR [Type]=(2))))
GO
IF  EXISTS (SELECT * FROM sys.check_constraints WHERE object_id = OBJECT_ID(N'[dbo].[CK_Questions]') AND parent_object_id = OBJECT_ID(N'[dbo].[Questions]'))
ALTER TABLE [dbo].[Questions] CHECK CONSTRAINT [CK_Questions]
GO
IF NOT EXISTS (SELECT * FROM sys.check_constraints WHERE object_id = OBJECT_ID(N'[dbo].[CK_Slider_Questions]') AND parent_object_id = OBJECT_ID(N'[dbo].[Slider_Questions]'))
ALTER TABLE [dbo].[Slider_Questions]  WITH CHECK ADD  CONSTRAINT [CK_Slider_Questions] CHECK  (([StartValue]>=(1) AND [EndValue]<=(100) AND [StartValue]<[EndValue] AND len([StartValueCaption])<=(100) AND len([EndValueCaption])<=(100)))
GO
IF  EXISTS (SELECT * FROM sys.check_constraints WHERE object_id = OBJECT_ID(N'[dbo].[CK_Slider_Questions]') AND parent_object_id = OBJECT_ID(N'[dbo].[Slider_Questions]'))
ALTER TABLE [dbo].[Slider_Questions] CHECK CONSTRAINT [CK_Slider_Questions]
GO
IF NOT EXISTS (SELECT * FROM sys.check_constraints WHERE object_id = OBJECT_ID(N'[dbo].[CK_Smiley_Questions]') AND parent_object_id = OBJECT_ID(N'[dbo].[Smiley_Questions]'))
ALTER TABLE [dbo].[Smiley_Questions]  WITH NOCHECK ADD  CONSTRAINT [CK_Smiley_Questions] CHECK  (([NumberOfSmileyFaces]>=(1) AND [NumberOfSmileyFaces]<=(5)))
GO
IF  EXISTS (SELECT * FROM sys.check_constraints WHERE object_id = OBJECT_ID(N'[dbo].[CK_Smiley_Questions]') AND parent_object_id = OBJECT_ID(N'[dbo].[Smiley_Questions]'))
ALTER TABLE [dbo].[Smiley_Questions] CHECK CONSTRAINT [CK_Smiley_Questions]
GO
IF NOT EXISTS (SELECT * FROM sys.check_constraints WHERE object_id = OBJECT_ID(N'[dbo].[CK_Star_Questions]') AND parent_object_id = OBJECT_ID(N'[dbo].[Star_Questions]'))
ALTER TABLE [dbo].[Star_Questions]  WITH CHECK ADD  CONSTRAINT [CK_Star_Questions] CHECK  (([NumberOfStars]>=(1) AND [NumberOfStars]<=(10)))
GO
IF  EXISTS (SELECT * FROM sys.check_constraints WHERE object_id = OBJECT_ID(N'[dbo].[CK_Star_Questions]') AND parent_object_id = OBJECT_ID(N'[dbo].[Star_Questions]'))
ALTER TABLE [dbo].[Star_Questions] CHECK CONSTRAINT [CK_Star_Questions]
GO
/****** Object:  StoredProcedure [dbo].[INSERT_SLIDER_QUESTION]    Script Date: 12/06/2022 12:20:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[INSERT_SLIDER_QUESTION]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[INSERT_SLIDER_QUESTION] AS' 
END
GO
ALTER PROCEDURE [dbo].[INSERT_SLIDER_QUESTION]
@Order INT,
@Text NVARCHAR(4000),
@Type INT,
@StartValue INT,
@EndValue INT,
@StartValueCaption nvarchar(100),
@EndValueCaption nvarchar(100)
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
GO
/****** Object:  StoredProcedure [dbo].[INSERT_SMILEY_QUESTION]    Script Date: 12/06/2022 12:20:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[INSERT_SMILEY_QUESTION]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[INSERT_SMILEY_QUESTION] AS' 
END
GO
ALTER PROCEDURE [dbo].[INSERT_SMILEY_QUESTION]
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
GO
/****** Object:  StoredProcedure [dbo].[INSERT_STAR_QUESTION]    Script Date: 12/06/2022 12:20:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[INSERT_STAR_QUESTION]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[INSERT_STAR_QUESTION] AS' 
END
GO
ALTER PROCEDURE [dbo].[INSERT_STAR_QUESTION]
@ORDER INT,
@TEXT NVARCHAR(4000),
@TYPE INT,
@NumberOfStars INT
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
	
	INSERT INTO Star_Questions(ID,  NumberOfStars)
	VALUES (SCOPE_IDENTITY(), @NumberOfStars)
	
	SELECT @SUCCESS AS ErrorCode, ERROR_MESSAGE() AS ErrorMessage
	RETURN @SUCCESS
END TRY
BEGIN CATCH
	SELECT @ERROR AS ErrorCode, ERROR_MESSAGE() AS ErrorMessage
	RETURN @ERROR
END CATCH
GO
/****** Object:  StoredProcedure [dbo].[UPDATE_SLIDER_QUESTION]    Script Date: 12/06/2022 12:20:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UPDATE_SLIDER_QUESTION]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[UPDATE_SLIDER_QUESTION] AS' 
END
GO
ALTER PROCEDURE [dbo].[UPDATE_SLIDER_QUESTION]
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
GO
/****** Object:  StoredProcedure [dbo].[UPDATE_SMILEY_QUESTION]    Script Date: 12/06/2022 12:20:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UPDATE_SMILEY_QUESTION]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[UPDATE_SMILEY_QUESTION] AS' 
END
GO
ALTER PROCEDURE [dbo].[UPDATE_SMILEY_QUESTION]
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
GO
/****** Object:  StoredProcedure [dbo].[UPDATE_STAR_QUESTION]    Script Date: 12/06/2022 12:20:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UPDATE_STAR_QUESTION]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[UPDATE_STAR_QUESTION] AS' 
END
GO
ALTER PROCEDURE [dbo].[UPDATE_STAR_QUESTION]
@ID INT,
@Order INT,
@Text NVARCHAR(4000),
@Type INT,
@NumberOfStars INT
AS
SET NOCOUNT ON

BEGIN TRY
	DECLARE @SUCCESS INT, @ERROR INT
	SET @SUCCESS = 1
	SET @ERROR = -1

	UPDATE Questions
	SET [Order] = @Order, [Text] = @Text
	WHERE [ID] = @ID

	UPDATE Star_Questions
	SET NumberOfStars = @NumberOfStars
	WHERE ID = @ID

	SELECT @SUCCESS AS ErrorCode, ERROR_MESSAGE() AS ErrorMessage
	RETURN @SUCCESS

END TRY
BEGIN CATCH
	SELECT @ERROR AS ErrorCode, ERROR_MESSAGE() AS ErrorMessage
	RETURN @ERROR
END CATCH
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Slider_Questions', N'CONSTRAINT',N'CK_Slider_Questions'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Check if start value is at least 1
end value is at max 100
start value < end value' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Slider_Questions', @level2type=N'CONSTRAINT',@level2name=N'CK_Slider_Questions'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Smiley_Questions', N'CONSTRAINT',N'CK_Smiley_Questions'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Check if value is between 1 and 10' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Smiley_Questions', @level2type=N'CONSTRAINT',@level2name=N'CK_Smiley_Questions'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Star_Questions', N'CONSTRAINT',N'CK_Star_Questions'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Check number of stars if between 1 and 10' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Star_Questions', @level2type=N'CONSTRAINT',@level2name=N'CK_Star_Questions'
GO
USE [master]
GO
ALTER DATABASE [SurveyQuestionsConfigurator] SET  READ_WRITE 
GO
