USE [master]
GO

/****** Object:  Database [SurveyQuestionsConfigurator]    Script Date: 24/05/2022 19:05:18 ******/
IF DB_ID(N'SurveyQuestionsConfigurator') IS NOT NULL
BEGIN
DROP DATABASE [SurveyQuestionsConfigurator]
END
GO

/****** Object:  Database [SurveyQuestionsConfigurator]    Script Date: 24/05/2022 19:05:18 ******/
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




/****** Object:  Table [dbo].[Slider_Questions]    Script Date: 24/05/2022 19:12:57 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Slider_Questions]') AND type in (N'U'))
DROP TABLE [dbo].[Slider_Questions]
GO

/****** Object:  Table [dbo].[Slider_Questions]    Script Date: 24/05/2022 19:12:57 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Slider_Questions](
	[QuestionID] [int] IDENTITY(1,1) NOT NULL,
	[QuestionOrder] [int] NOT NULL,
	[QuestionText] [text] NOT NULL,
	[QuestionStartValue] [int] NOT NULL,
	[QuestionEndValue] [int] NOT NULL,
	[QuestionStartValueCaption] [varchar](100) NOT NULL,
	[QuestionEndValueCaption] [varchar](100) NOT NULL,
 CONSTRAINT [PK_Slider_Questions] PRIMARY KEY CLUSTERED 
(
	[QuestionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [IX_Slider_Questions] UNIQUE NONCLUSTERED 
(
	[QuestionOrder] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO





/****** Object:  Table [dbo].[Smiley_Questions]    Script Date: 24/05/2022 19:13:25 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Smiley_Questions]') AND type in (N'U'))
DROP TABLE [dbo].[Smiley_Questions]
GO

/****** Object:  Table [dbo].[Smiley_Questions]    Script Date: 24/05/2022 19:13:25 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Smiley_Questions](
	[QuestionID] [int] IDENTITY(1,1) NOT NULL,
	[QuestionOrder] [int] NOT NULL,
	[QuestionText] [text] NOT NULL,
	[NumberOfSmileyFaces] [int] NOT NULL,
 CONSTRAINT [PK_Smiley_Faces] PRIMARY KEY CLUSTERED 
(
	[QuestionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO





/****** Object:  Table [dbo].[Star_Questions]    Script Date: 24/05/2022 19:13:43 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Star_Questions]') AND type in (N'U'))
DROP TABLE [dbo].[Star_Questions]
GO

/****** Object:  Table [dbo].[Star_Questions]    Script Date: 24/05/2022 19:13:43 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Star_Questions](
	[QuestionID] [int] IDENTITY(1,1) NOT NULL,
	[QuestionOrder] [int] NOT NULL,
	[QuestionText] [text] NOT NULL,
	[NumberOfStars] [int] NOT NULL,
 CONSTRAINT [PK_Stars_Questions] PRIMARY KEY CLUSTERED 
(
	[QuestionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [IX_Stars_Questions] UNIQUE NONCLUSTERED 
(
	[QuestionOrder] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO





