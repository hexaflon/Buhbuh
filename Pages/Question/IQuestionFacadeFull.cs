namespace ProjektInzynierski.Pages.Question
{
    using TestTest.Models.Db;
    public interface IQuestionFacade
    {
        Question CreateQuestion(string text, int teacherId, int? categoryId, int questionType, bool isTrueFalse);
        List<Question> GetQuestionList(string? searchText = null, int? categoryId = null, int? typeId = null);
        void AddTrueFalseAnswers(int questionId, bool isTrueFalse);
    }
}