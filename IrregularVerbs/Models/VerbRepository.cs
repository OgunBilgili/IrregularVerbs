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

            var data = _context.Results.Where(x => x.TimeStamp == TimeStamp).FirstOrDefault();

            // Check wheter session's first object added to DB or not.
            if(data != null)
            {
                // If the submitted form is correct
                if (answerKey.BaseForm == submit.BaseForm && answerKey.PastSimple == submit.PastSimple && answerKey.PastParticiple == submit.PastParticiple)
                {
                    data.NumberofCorrectAnswers += 1;
                }
                //If the submitted form is Incorrect
                else
                {
                    var incorrect = MakeIncorrectObject(TimeStamp, 0, givenVerb, submittedAnswerFirst, submittedAnswerSecond,
                                                                            correctAnswerFirst, correctAnswerSecond);

                    data.NumberofIncorrectAnswers += 1;

                    _context.Incorrects.Add(incorrect);
                }
            }
            else
            {
                //If the submitted form is correct
                if (answerKey.BaseForm == submit.BaseForm && answerKey.PastSimple == submit.PastSimple && answerKey.PastParticiple == submit.PastParticiple)
                {
                    var result = MakeResultObject(TimeStamp, 1, 0, 1);
                    _context.Results.Add(result);
                }
                //If the submitted form is Incorrect
                else
                {
                    var result = MakeResultObject(TimeStamp, 0, 1, 1);

                    var incorrect = MakeIncorrectObject(TimeStamp, 0, givenVerb, submittedAnswerFirst, submittedAnswerSecond, 
                                                        correctAnswerFirst, correctAnswerSecond);

                    _context.Incorrects.Add(incorrect);
                    _context.Results.Add(result);
                }
            }

            // Save changes to DB
            _context.SaveChanges();
        }

        public Result MakeResultObject(DateTime? TimeStamp, int? NumberofCorrectAnswers, int? NumberofIncorrectAnswers, int? Accuracy)
        {
            Result result = new Result
            {
                TimeStamp = TimeStamp,
                NumberofCorrectAnswers = NumberofCorrectAnswers,
                NumberofIncorrectAnswers = NumberofIncorrectAnswers,
                Accuracy = Accuracy
            };

            return result;
        }
            
        public Incorrect MakeIncorrectObject(DateTime? TimeStamp, int Checked, string givenVerb, string submittedAnswerFirst, string submittedAnswerSecond, 
                                             string correctAnswerFirst, string correctAnswerSecond)
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

            return incorrect;
        }

        // Get Wrong Answers
        public List<IncorrectForm> GetIncorrectSubmissions()
        {
            var data = (from x in _context.Incorrects
                        select new IncorrectForm
                        {
                            TimeStamp = x.TimeStamp,
                            GivenVerb = x.GivenVerb,
                            CorrectAnswerFirst = x.CorrectAnswerFirst,
                            CorrectAnswerSecond = x.CorrectAnswerSecond,
                            SubmittedAnswerFirst = x.SubmittedAnswerFirst,
                            SubmittedAnswerSecond = x.SubmittedAnswerSecond,
                            Checked = x.Checked
                        }).ToList();
            return data;
        }
    }
}
