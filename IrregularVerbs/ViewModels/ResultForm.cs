using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IrregularVerbs.ViewModels
{
    public class ResultForm
    {
        public DateTime? TimeStamp { get; set; }
        public int? NumberofCorrectAnswers { get; set; }
        public int? NumberofIncorrectAnswers { get; set; }
        public int? Accuracy { get; set; }
        public List<ResultForm> ResultFormList { get; set; }
    }
}
