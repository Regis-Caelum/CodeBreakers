using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Source.Models
{
    public class AdminModel
    {
        public string Name { get; set; }

        public string PhoneNo { get; set; }

        public string Department { get; set; }

        public string StudentEmail { get; set; }

        public string UpdateTask { get; set; }
    }
}
