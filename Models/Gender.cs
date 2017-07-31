using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NextekkManagmt.Models
{
    public class Gender
    {
        public Guid ID { get; set; }
        public string GenderType { get; internal set; }
    }
}