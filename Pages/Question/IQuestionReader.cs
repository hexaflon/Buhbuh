

namespace ProjektInzynierski.Pages.Question
{
    using TestTest.Models.Db;
    public interface IQuestionReader
    {
        List<Question> GetQuestionList(string? searchText = null, int? categoryId = null, int? typeId = null);
    }

}