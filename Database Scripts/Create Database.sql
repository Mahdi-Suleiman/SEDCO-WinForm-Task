USE [master]
GO

/****** Object:  Database [SurveyQuestionsConfigurator]    Script Date: 05/06/2022 20:05:36 ******/
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


----------------------------------------------------------------
USE [SurveyQuestionsConfigurator]
GO

/****** Object:  Table [dbo].[Smiley_Questions]    Script Date: 05/06/2022 20:07:35 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Smiley_Questions](
	[ID] [int] NOT NULL,
	[NumberOfSmileyFaces] [int] NOT NULL,
 CONSTRAINT [IX_Smiley_Questions] UNIQUE NONCLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Smiley_Questions]  WITH CHECK ADD  CONSTRAINT [FK_Smiley_Questions_Questions] FOREIGN KEY([ID])
REFERENCES [dbo].[Questions] ([ID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[Smiley_Questions] CHECK CONSTRAINT [FK_Smiley_Questions_Questions]
GO

ALTER TABLE [dbo].[Smiley_Questions]  WITH NOCHECK ADD  CONSTRAINT [CK_Smiley_Questions] CHECK  (([NumberOfSmileyFaces]>=(1) AND [NumberOfSmileyFaces]<=(5)))
GO

ALTER TABLE [dbo].[Smiley_Questions] CHECK CONSTRAINT [CK_Smiley_Questions]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Check if value is between 1 and 10' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Smiley_Questions', @level2type=N'CONSTRAINT',@level2name=N'CK_Smiley_Questions'
GO

----------------------------------------------------------------
USE [SurveyQuestionsConfigurator]
GO

/****** Object:  Table [dbo].[Slider_Questions]    Script Date: 05/06/2022 20:07:52 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Slider_Questions](
	[ID] [int] NOT NULL,
	[StartValue] [int] NOT NULL,
	[EndValue] [int] NOT NULL,
	[StartValueCaption] [varchar](100) NOT NULL,
	[EndValueCaption] [varchar](100) NOT NULL,
 CONSTRAINT [IX_Slider_Questions] UNIQUE NONCLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Slider_Questions]  WITH CHECK ADD  CONSTRAINT [FK_Slider_Questions_Questions] FOREIGN KEY([ID])
REFERENCES [dbo].[Questions] ([ID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[Slider_Questions] CHECK CONSTRAINT [FK_Slider_Questions_Questions]
GO

ALTER TABLE [dbo].[Slider_Questions]  WITH CHECK ADD  CONSTRAINT [CK_Slider_Questions] CHECK  (([StartValue]>=(1) AND [EndValue]<=(100) AND [StartValue]<[EndValue] AND datalength([StartValueCaption])<=(100) AND datalength([EndValueCaption])<=(100)))
GO

ALTER TABLE [dbo].[Slider_Questions] CHECK CONSTRAINT [CK_Slider_Questions]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Check if start value is at least 1
end value is at max 100
start value < end value' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Slider_Questions', @level2type=N'CONSTRAINT',@level2name=N'CK_Slider_Questions'
GO


----------------------------------------------------------------
USE [SurveyQuestionsConfigurator]
GO

/****** Object:  Table [dbo].[Star_Questions]    Script Date: 05/06/2022 20:08:12 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Star_Questions](
	[ID] [int] NOT NULL,
	[NumberOfStars] [int] NOT NULL,
 CONSTRAINT [IX_Star_Questions] UNIQUE NONCLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Star_Questions]  WITH CHECK ADD  CONSTRAINT [FK_Star_Questions_Questions] FOREIGN KEY([ID])
REFERENCES [dbo].[Questions] ([ID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[Star_Questions] CHECK CONSTRAINT [FK_Star_Questions_Questions]
GO

ALTER TABLE [dbo].[Star_Questions]  WITH CHECK ADD  CONSTRAINT [CK_Star_Questions] CHECK  (([NumberOfStars]>=(1) AND [NumberOfStars]<=(10)))
GO

ALTER TABLE [dbo].[Star_Questions] CHECK CONSTRAINT [CK_Star_Questions]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Check number of stars if between 1 and 10' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Star_Questions', @level2type=N'CONSTRAINT',@level2name=N'CK_Star_Questions'
GO


