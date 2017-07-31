using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NextekkManagmt.Models
{
    public class MaritalStatus
    {
        public Guid ID { get; set; }
        public string Status { get; set; }
        public int order { get; set; }
    }
}