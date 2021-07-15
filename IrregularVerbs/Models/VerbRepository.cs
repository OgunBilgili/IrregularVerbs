using IrregularVerbs.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IrregularVerbs.Models
{
    public class VerbRepository : IVerbRepository
    {
        private readonly IrregularVerbsContext _context;
        

        public VerbRepository(IrregularVerbsContext context)
        {
            _context = context;
        }

        public SubmitForm GetVerb(Irregular verb, int verbsLeft)
        {
            SubmitForm submit = null;
            Random random = new Random();
            int selected = 0;

            selected = random.Next(1, 3);
            switch (selected)
            {
                case 1:
                    submit = fillSubmission(verbsLeft,"BaseForm", verb.BaseForm, null, null);
                    break;
                case 2:
                    submit = fillSubmission(verbsLeft,"PastSimple", null, verb.PastSimple, null);
                    break;
                case 3:
                    submit = fillSubmission(verbsLeft,"PastParticiple", null, null, verb.PastParticiple);
                    break;
            }

            return submit;
        }

        public SubmitForm fillSubmission(int VerbsLeft, string GivenVerbType, string BaseForm, string PastSimple, string PastParticiple)
        {
            SubmitForm submit = new SubmitForm
            {
                VerbsLeft = VerbsLeft,
                GivenVerbType = GivenVerbType,
                BaseForm = BaseForm,
                PastSimple = PastSimple,
                PastParticiple = PastParticiple
            };

            return submit;
        }

        public void CheckSubmittedForm(DateTime TimeStamp, SubmitForm submit)
        {
            Irregular answerKey = null;
            string givenVerb = string.Empty, submittedAnswerFirst = string.Empty, submittedAnswerSecond = string.Empty,
                    correctAnswerFirst = string.Empty, correctAnswerSecond = string.Empty;

            //Get the Answers
            if (submit.GivenVerbType == "BaseForm")
            {
                givenVerb = submit.BaseForm;
                submittedAnswerFirst = submit.PastSimple;
                submittedAnswerSecond = submit.PastParticiple;
                answerKey = _context.Irregulars.Where(x => x.BaseForm == submit.BaseForm).FirstOrDefault();
                correctAnswerFirst = answerKey.PastSimple;
                correctAnswerSecond = answerKey.PastParticiple;
            }
            else if (submit.GivenVerbType == "PastSimple")
            {
                givenVerb = submit.PastSimple;
                submittedAnswerFirst = submit.BaseForm;
                submittedAnswerSecond = submit.PastParticiple;
                answerKey = _context.Irregulars.Where(x => x.PastSimple == submit.PastSimple).FirstOrDefault();
                correctAnswerFirst = answerKey.BaseForm;
                correctAnswerSecond = answerKey.PastParticiple;
            }
            else
            {
                givenVerb = submit.PastParticiple;
                submittedAnswerFirst = submit.BaseForm;
                submittedAnswerSecond = submit.PastSimple;
                answerKey = _context.Irregulars.Where(x => x.PastParticiple == submit.PastParticiple).FirstOrDefault();
                correctAnswerFirst = answerKey.BaseForm;
                correctAnswerSecond = answerKey.PastSimple;
            }
                
            //If the submitted form is correct
            if(answerKey.BaseForm == submit.BaseForm && answerKey.PastSimple == submit.PastSimple && answerKey.PastParticiple == submit.PastParticiple)
            {
                Result result = new Result
                {
                    TimeStamp = TimeStamp,
                    NumberofCorrectAnswers = 1,
                    NumberofIncorrectAnswers = 0,
                    Accuracy = 1                    
                };

                _context.Results.Add(result);
                _context.SaveChanges();
            }
            else
            {
                Incorrect incorrect = new Incorrect
                {
                    TimeStamp = TimeStamp,
                    Checked = 0,
                    GivenVerb = givenVerb,
                    SubmittedAnswerFirst = submittedAnswerFirst,
                    SubmittedAnswerSecond = submittedAnswerSecond,
                    CorrectAnswerFirst = correctAnswerFirst,
                    CorrectAnswerSecond = correctAnswerSecond
                };

                _context.Incorrects.Add(incorrect);
                _context.SaveChanges();
            }
        }
    }
}
