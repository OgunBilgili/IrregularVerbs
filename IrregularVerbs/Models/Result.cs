using System;
using System.Collections.Generic;

#nullable disable

namespace IrregularVerbs.Models
{
    public partial class Result
    {
        public int Id { get; set; }
        public DateTime? TimeStamp { get; set; }
        public int? NumberofCorrectAnswers { get; set; }
        public int? NumberofIncorrectAnswers { get; set; }
        public int? Accuracy { get; set; }
    }
}
