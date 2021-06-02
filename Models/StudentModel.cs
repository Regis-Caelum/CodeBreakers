using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Source.Models
{
    public class StudentModel
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string PhoneNo { get; set; }

        public string Department { get; set; }

        public string Task { get; set; }

    }
}
