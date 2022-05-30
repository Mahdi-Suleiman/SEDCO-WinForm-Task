USE [master]
GO

/****** Object:  Database [SurveyQuestionsConfigurator]    Script Date: 30/05/2022 14:55:18 ******/
CREATE DATABASE [SurveyQuestionsConfigurator]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'SurveyQuestionsConfigurator', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\SurveyQuestionsConfigurator.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'SurveyQuestionsConfigurator_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\SurveyQuestionsConfigurator_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
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

ALTER DATABASE [SurveyQuestionsConfigurator] SET  DISABLE_BROKER 
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

ALTER DATABASE [SurveyQuestionsConfigurator] SET QUERY_STORE = OFF
GO

ALTER DATABASE [SurveyQuestionsConfigurator] SET  READ_WRITE 
GO


USE [SurveyQuestionsConfigurator]
GO

/****** Object:  Table [dbo].[Questions]    Script Date: 30/05/2022 14:45:12 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Questions](
	[QuestionID] [int] IDENTITY(1,1) NOT NULL,
	[QuestionOrder] [int] NOT NULL,
	[QuestionText] [varchar](8000) NOT NULL,
	[QuestionType] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Questions_1] PRIMARY KEY CLUSTERED 
(
	[QuestionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [IX_Questions] UNIQUE NONCLUSTERED 
(
	[QuestionOrder] ASC,
	[QuestionType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Questions]  WITH CHECK ADD  CONSTRAINT [CK_QuestionType] CHECK  (([QuestionType] like 'SMILEY' OR [QuestionType] like 'SLIDER' OR [QuestionType] like 'STAR'))
GO

ALTER TABLE [dbo].[Questions] CHECK CONSTRAINT [CK_QuestionType]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Check if the type is either ''Smiley'' or ''Slider'' or ''Star''' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Questions', @level2type=N'CONSTRAINT',@level2name=N'CK_QuestionType'
GO


USE [SurveyQuestionsConfigurator]
GO

/****** Object:  Table [dbo].[Slider_Questions]    Script Date: 30/05/2022 14:45:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Slider_Questions](
	[QuestionID] [int] NOT NULL,
	[QuestionStartValue] [int] NOT NULL,
	[QuestionEndValue] [int] NOT NULL,
	[QuestionStartValueCaption] [varchar](100) NOT NULL,
	[QuestionEndValueCaption] [varchar](100) NOT NULL,
 CONSTRAINT [IX_Slider_Questions] UNIQUE NONCLUSTERED 
(
	[QuestionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Slider_Questions]  WITH CHECK ADD  CONSTRAINT [FK_Slider_Questions_Questions] FOREIGN KEY([QuestionID])
REFERENCES [dbo].[Questions] ([QuestionID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[Slider_Questions] CHECK CONSTRAINT [FK_Slider_Questions_Questions]
GO

ALTER TABLE [dbo].[Slider_Questions]  WITH CHECK ADD  CONSTRAINT [CK_Slider_Questions] CHECK  (([QuestionStartValue]>=(1) AND [QuestionEndValue]<=(100) AND [QuestionStartValue]<[QuestionEndValue]))
GO

ALTER TABLE [dbo].[Slider_Questions] CHECK CONSTRAINT [CK_Slider_Questions]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Check if start value is at least 1
end value is at max 100
start value < end value' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Slider_Questions', @level2type=N'CONSTRAINT',@level2name=N'CK_Slider_Questions'
GO


USE [SurveyQuestionsConfigurator]
GO

/****** Object:  Table [dbo].[Smiley_Questions]    Script Date: 30/05/2022 14:45:35 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Smiley_Questions](
	[QuestionID] [int] NOT NULL,
	[NumberOfSmileyFaces] [int] NOT NULL,
 CONSTRAINT [IX_Smiley_Questions] UNIQUE NONCLUSTERED 
(
	[QuestionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Smiley_Questions]  WITH CHECK ADD  CONSTRAINT [FK_Smiley_Questions_Questions] FOREIGN KEY([QuestionID])
REFERENCES [dbo].[Questions] ([QuestionID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[Smiley_Questions] CHECK CONSTRAINT [FK_Smiley_Questions_Questions]
GO

ALTER TABLE [dbo].[Smiley_Questions]  WITH CHECK ADD  CONSTRAINT [CK_Smiley_Questions] CHECK  (([NumberOfSmileyFaces]>=(1) AND [NumberOfSmileyFaces]<=(10)))
GO

ALTER TABLE [dbo].[Smiley_Questions] CHECK CONSTRAINT [CK_Smiley_Questions]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Check if value is between 1 and 10' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Smiley_Questions', @level2type=N'CONSTRAINT',@level2name=N'CK_Smiley_Questions'
GO


USE [SurveyQuestionsConfigurator]
GO

/****** Object:  Table [dbo].[Star_Questions]    Script Date: 30/05/2022 14:45:45 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Star_Questions](
	[QuestionID] [int] NOT NULL,
	[NumberOfStars] [int] NOT NULL,
 CONSTRAINT [IX_Star_Questions] UNIQUE NONCLUSTERED 
(
	[QuestionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Star_Questions]  WITH CHECK ADD  CONSTRAINT [FK_Star_Questions_Questions] FOREIGN KEY([QuestionID])
REFERENCES [dbo].[Questions] ([QuestionID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[Star_Questions] CHECK CONSTRAINT [FK_Star_Questions_Questions]
GO

ALTER TABLE [dbo].[Star_Questions]  WITH CHECK ADD  CONSTRAINT [CK_Star_Questions] CHECK  (([NumberOfStars]>=(1) AND [NumberOfStars]<=(10)))
GO

ALTER TABLE [dbo].[Star_Questions] CHECK CONSTRAINT [CK_Star_Questions]
GO


