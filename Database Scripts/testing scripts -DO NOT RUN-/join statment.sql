use SurveyQuestionsTest
select Q.QuestionOrder, Q.QuestionText, StQ.NumberOfStars
from Questions AS Q
inner join Star_Questions AS StQ
on Q.QuestionID = StQ.QuestionID
where Q.QuestionID = 21