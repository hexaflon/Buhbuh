namespace ProjektInzynierski.Pages.Question
{
    public interface ITrueFalseAnswerGenerator
    {
        void AddTrueFalseAnswers(int questionId, bool isTrueFalse);
    }
}