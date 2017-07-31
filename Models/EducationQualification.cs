using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NextekkManagmt.Models
{
    public class EducationQualification
    {
        public Guid ID { get; set; }
        public string Level { get; set; }
        public int order { get; set; }
    }
}