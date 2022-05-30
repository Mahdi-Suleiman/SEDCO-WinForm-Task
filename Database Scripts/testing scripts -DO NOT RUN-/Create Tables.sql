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


