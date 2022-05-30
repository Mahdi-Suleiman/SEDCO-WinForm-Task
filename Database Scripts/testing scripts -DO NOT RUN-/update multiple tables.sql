USE SurveyQuestionsTest
UPDATE Questions
SET QuestionOrder = 5, QuestionText = '{}'
WHERE QuestionID = 5

UPDATE Star_Questions
SET NumberOfStars = 5
WHERE QuestionID = 5