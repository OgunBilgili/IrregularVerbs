using System;
using System.Collections.Generic;

#nullable disable

namespace IrregularVerbs.Models
{
    public partial class Irregular
    {
        public int Id { get; set; }
        public string BaseForm { get; set; }
        public string PastSimple { get; set; }
        public string PastParticiple { get; set; }
    }
}
