using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IrregularVerbs.ViewModels
{
    public class IncorrectForm
    {
        public DateTime? TimeStamp { get; set; }
        public int? Checked { get; set; }
        public string GivenVerb { get; set; }
        public string SubmittedAnswerFirst { get; set; }
        public string SubmittedAnswerSecond { get; set; }
        public string CorrectAnswerFirst { get; set; }
        public string CorrectAnswerSecond { get; set; }
        public List<IncorrectForm> IncorrectFormList { get; set; }
    }
}
