using IrregularVerbs.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IrregularVerbs.Models
{
    public interface IVerbRepository
    {
        SubmitForm GetVerb(Irregular verb, int verbsLeft);
        void CheckSubmittedForm(DateTime TimeStamp, SubmitForm submit);
        List<IncorrectForm> GetIncorrectSubmissions();
        public Result MakeResultObject(DateTime? TimeStamp, int? NumberofCorrectAnswers, int? NumberofIncorrectAnswers, int? Accuracy);
        public Incorrect MakeIncorrectObject(DateTime? TimeStamp, int Checked, string givenVerb, string submittedAnswerFirst, 
                                            string submittedAnswerSecond, string correctAnswerFirst, string correctAnswerSecond);
        public List<ResultForm> GetResults();
        public List<IncorrectForm> GetSpecificResult(DateTime TimeStamp);
    }
}
