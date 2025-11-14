namespace ProjektInzynierski.Pages.Question
{
    using TestTest.Models.Db;


    public interface IQuestionCreator
    {
        Question CreateQuestion(string text, int teacherId, int? categoryId, int questionType, bool isTrueFalse);
    }


}