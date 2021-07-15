using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IrregularVerbs.ViewModels
{
    public class SubmitForm
    {
        public int VerbsLeft { get; set; }
        public string GivenVerbType { get; set; }
        public string BaseForm { get; set; }
        public string PastSimple { get; set; }
        public string PastParticiple { get; set; }

    }
}
