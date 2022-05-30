USE SurveyQuestionsTest
INSERT INTO Questions
(QuestionOrder, QuestionText, QuestionType)
VALUES
(3, '{QuestionText}', 'SMILEY')
INSERT INTO Smiley_Questions
(QuestionID, NumberOfSmileyFaces)
VALUES
((SELECT QuestionID FROM Questions WHERE QuestionOrder = 3 AND QuestionType = 'SMILEY') , 2)