using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone62.Models
{
    public class Student
    {
        public int StudentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }
        public bool IsAdmin { get; set; }


        public ICollection<Enrollment> Enrollment { get; set; }

        public Student()
        {
            IsAdmin = false;
            Role = "Student";
        }
    }
}
