using TestTest.Models.Db;

namespace ProjektInzynierski.Pages.Answer
{
    public static class AnswerFactory
    {
        public static TestTest.Models.Db.Answer Create(int questionId, string text,bool isCorrect, int AnswerId)
        {
            var answer = new TestTest.Models.Db.Answer();
            answer.Text = text;
            answer.QuestionId = questionId;
            answer.Id = AnswerId;

            switch (text)
            {
                //Dla pytań Prawda/Fałsz argument czyPoprawny można interpretować jako:
                //czy Prawda to poprawna odpowiedź
                case "Prawda":
                    {
                        if (isCorrect)answer.IsCorrect = true;
                        else { answer.IsCorrect = false; }
                        break;
                    }
                case "Fałsz":
                    {
                        if (isCorrect) answer.IsCorrect = false;
                        else answer.IsCorrect = true;
                        break;
                    }
                default:
                    {
                        answer.IsCorrect = isCorrect;
                        break;
                    }
            }
            
            return answer;
        }
    }
}
