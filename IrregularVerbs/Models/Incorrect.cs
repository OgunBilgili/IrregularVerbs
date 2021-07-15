using System;
using System.Collections.Generic;

#nullable disable

namespace IrregularVerbs.Models
{
    public partial class Incorrect
    {
        public int Id { get; set; }
        public DateTime? TimeStamp { get; set; }
        public int? Checked { get; set; }
        public string GivenVerb { get; set; }
        public string SubmittedAnswerFirst { get; set; }
        public string SubmittedAnswerSecond { get; set; }
        public string CorrectAnswerFirst { get; set; }
        public string CorrectAnswerSecond { get; set; }
    }
}
