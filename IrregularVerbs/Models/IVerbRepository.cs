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
    }
}
