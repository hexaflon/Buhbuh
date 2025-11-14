using TestTest.Models.Db;

namespace ProjektInzynierski.Pages.Question
{
    public class ChoiceQuestion : QuestionTemplate
    {
        private readonly int _typeId;

        public ChoiceQuestion(DatabaseContext context, int typeId) : base(context)
        {
            _typeId = typeId;
        }

        protected override TestTest.Models.Db.Question PrepareQuestion(string text, int teacherId, int? categoryId)
        {
            return new TestTest.Models.Db.Question
            {
                Text = text,
                TeacherId = teacherId,
                CategoryId = categoryId,
                TypeId = _typeId
            };
        }

        protected override void AddAnswers(TestTest.Models.Db.Question question)
        {
        }
    }
}
